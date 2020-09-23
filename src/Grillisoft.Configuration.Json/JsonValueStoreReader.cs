using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Grillisoft.Configuration.Stores;

namespace Grillisoft.Configuration.Json
{
    public class JsonValueStoreReader : IValuesStoreReader
    {
        public async Task<IValuesStore> Load(string folder, string name)
        {
            var model = await LoadInternal(Path.Combine(folder, name + ".json"));
            var parent = await LoadParent(folder, model.Parent);

            return new MemoryValuesStore(model.Keys, parent);
        }

        private async Task<JsonStoreModel> LoadInternal(string path)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/using-async-for-file-access
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                return await JsonSerializer.DeserializeAsync<JsonStoreModel>(stream, Options);
            }
        }

        private async Task<IValuesStore> LoadParent(string folder, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            return await this.Load(folder, name);
        }

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}