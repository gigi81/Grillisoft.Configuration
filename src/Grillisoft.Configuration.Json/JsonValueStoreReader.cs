using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Grillisoft.Configuration.Stores;

namespace Grillisoft.Configuration.Json
{
    public class JsonValueStoreReader
    {
        public static async Task<MemoryValuesStore> Load(FileInfo file)
        {
            using (var stream = file.OpenRead())
            {
                var model = await JsonSerializer.DeserializeAsync<JsonStoreModel>(stream, Options);
                var parent = await LoadParent(file.Directory.FullName, model.Parent);

                return new MemoryValuesStore(model.Keys, parent);
            }
        }

        private static async Task<MemoryValuesStore> LoadParent(string folder, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            return await Load(new FileInfo(Path.Combine(folder, name + ".json")));
        }

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}