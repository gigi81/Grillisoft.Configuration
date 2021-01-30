using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Grillisoft.Configuration.Json
{
    public class JsonTreeConfigurationProvider : FileTreeConfigurationProvider<JsonTreeConfigurationSource>
    {
        /// <summary>
        /// Initializes a new instance with the specified source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public JsonTreeConfigurationProvider(JsonTreeConfigurationSource source) : base(source) { }

        public override IDictionary<string, string> Load(Stream stream, out string parent)
        {
            var model = JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream, Options).Result;
            if(model.TryGetValue(this.Source.ParentKey, out parent))
                model.Remove(this.Source.ParentKey);

            return model;
        }

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
}