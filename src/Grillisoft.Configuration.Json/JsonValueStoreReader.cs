namespace src.Grillisoft.Configuration.Json
{
    public class JsonValueStoreReader
    {
        public static async Task<MemoryValuesStore> Load(FileInfo file)
        {
            using (var stream = file.OpenRead())
            {
                var jsonModel = JsonSerializer.Deserialize<JsonStoreModel>(stream, Options);
            }
        }

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}