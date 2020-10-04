using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Grillisoft.Configuration.Json
{
    public class JsonTreeConfigurationProvider : FileTreeConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance with the specified source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public JsonTreeConfigurationProvider(JsonTreeConfigurationSource source) : base(source) { }

        public override IDictionary<string, string> Load(Stream stream, out string parentKey)
        {
            var model = JsonSerializer.DeserializeAsync<JsonStoreModel>(stream, Options).Result;
            parentKey = model.Parent;
            return model.Keys;
        }

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}