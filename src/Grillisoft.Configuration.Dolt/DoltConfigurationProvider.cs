using CsvHelper.Configuration;
using Grillisoft.Configuration.Dolt;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Grillisoft.Configuration
{
    internal class DoltConfigurationProvider : ConfigurationProvider
    {
        private const string ParentKey = ".parent";

        public DoltConfigurationProvider(DoltConfigurationSource source)
        {
            this.Source = source;
        }

        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public DoltConfigurationSource Source { get; }

        public override void Load()
        {
            var tempFolder = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "dolt_" + Guid.NewGuid()));

            try
            {
                tempFolder.Create();

                var runner = new DoltRunner(tempFolder.FullName).Clone(this.Source.Url, this.Source.Branch);
                var tables = runner.ListTables().Select(t => t.Name).ToList();
                var table = GetExistingTable(tables);

                Data = LoadInternal(runner, table, out var parentKey);

                while (!String.IsNullOrEmpty(parentKey))
                {
                    foreach (var pair in LoadInternal(runner, parentKey, out parentKey))
                    {
                        if (!Data.ContainsKey(pair.Key))
                            Data.Add(pair);
                    }
                }
            }
            finally
            {
                if (tempFolder.Exists)
                    RunWithRetry(() => tempFolder.Delete(recursive: true), 500);
            }
        }

        private void RunWithRetry(Action action, int sleep, int maxRetries = 10)
        {
            var retries = 0;

            while(retries < maxRetries)
            {
                try
                {
                    action.Invoke();
                    return;
                }
                catch(Exception)
                {
                    retries++;
                    if (retries >= maxRetries)
                        throw;

                    Thread.Sleep(sleep);
                }
            }
        }

        private string GetExistingTable(List<string> tables)
        {
            foreach (var key in this.Source.Keys)
            {
                var table = tables.FirstOrDefault(t => t.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if (!String.IsNullOrEmpty(table))
                    return table;
            }

            throw new Exception($"Could not find any table named {String.Join(" or ", this.Source.Keys)} in dolt repo {this.Source.Url}");
        }

        private static readonly ClassMap DefaultClassMap = new KeyValueRowMap();

        private IDictionary<string, string> LoadInternal(DoltRunner runner, string table, out string parentKey)
        {
            var ret = runner.Export<KeyValueRow>(table, DefaultClassMap).ToDictionary(r => r.Key, r => r.Value);
            ret.TryGetValue(ParentKey, out parentKey);
            return ret;
        }
    }
}