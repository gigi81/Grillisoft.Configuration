using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Grillisoft.Configuration.Stores;

namespace Grillisoft.Configuration.Json
{
    public class JsonValueStoreReader : IValueStoreReader
    {
        public async Task<IValueStore> Load(string folder, IEnumerable<string> keys)
        {
            var missing = new List<string>();

            foreach (var key in keys)
            {
                var file = GetFile(folder, key);
                if (file.Exists)
                    return await LoadFile(file);

                missing.Add(key);
            }

            throw new FileNotFoundException($"Keys '{String.Join(", ", missing)}' could not be found in '{folder}'");
        }

        private static FileInfo GetFile(string folder, string key)
        {
            return new FileInfo(Path.Combine(folder, key + ".json"));
        }

        private async Task<IValueStore> LoadFile(FileInfo file)
        {
            var model = await Deserialize(file.FullName);
            var parent = await LoadParent(file.DirectoryName, model.Parent);

            return new MemoryValueStore(model.Keys, parent);
        }

        private async Task<JsonStoreModel> Deserialize(string path)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/using-async-for-file-access
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                return await JsonSerializer.DeserializeAsync<JsonStoreModel>(stream, Options);
            }
        }

        private async Task<IValueStore> LoadParent(string folder, string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return null;

            return await LoadFile(GetFile(folder, key));
        }

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}