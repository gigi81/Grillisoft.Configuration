using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration
{
    public class DoltTreeConfigurationProvider : ConfigurationProvider
    {
        public DoltTreeConfigurationProvider(DoltTreeConfigurationSource source)
        {
            this.Source = source;
        }

        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public DoltTreeConfigurationSource Source { get; }

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
                Task.Run(() => {
                    RunWithRetry(() =>
                    {
                        if (tempFolder.Exists)
                            tempFolder.Delete(recursive: true);
                    }, 700);
                });
            }
        }

        private void RunWithRetry(Action action, int sleep, int maxRetries = 10, bool throwException = false)
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
                    if (retries >= maxRetries && throwException)
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

        private IDictionary<string, string> LoadInternal(DoltRunner runner, string table, out string parentKey)
        {
            var ret = runner.Export<KeyValueRow>(table, new KeyValueRowMap(this.Source.KeyField, this.Source.ValueField)).ToDictionary(r => r.Key, r => r.Value);
            ret.TryGetValue(this.Source.ParentKey, out parentKey);
            return ret;
        }
    }
}