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
            using (var stream = File.OpenRead(Path.Combine(folder, name + ".json")))
            {
                var model = await JsonSerializer.DeserializeAsync<JsonStoreModel>(stream, Options);
                var parent = await LoadParent(folder, model.Parent);

                return new MemoryValuesStore(model.Keys, parent);
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