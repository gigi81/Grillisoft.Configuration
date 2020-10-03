using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration.Json
{
    /// <summary>
    /// Represents a JSON file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class JsonKeyConfigurationSource : FileKeyConfigurationSource
    {
        public override string Extension => "json";

        /// <summary>
        /// Builds the <see cref="JsonKeyConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="JsonKeyConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new JsonKeyConfigurationProvider(this);
        }
    }
}
