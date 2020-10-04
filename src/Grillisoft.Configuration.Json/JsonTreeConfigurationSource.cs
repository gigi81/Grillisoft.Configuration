using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration.Json
{
    /// <summary>
    /// Represents a JSON file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class JsonTreeConfigurationSource : FileTreeConfigurationSource
    {
        public override string Extension => "json";

        /// <summary>
        /// Builds the <see cref="JsonTreeConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="JsonTreeConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new JsonTreeConfigurationProvider(this);
        }
    }
}
