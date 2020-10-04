using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration.Xml
{
    /// <summary>
    /// Represents a XML file as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class XmlTreeConfigurationSource : FileTreeConfigurationSource
    {
        public override string Extension => "xml";

        /// <summary>
        /// Builds the <see cref="JsonKeyConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="JsonKeyConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new XmlTreeConfigurationProvider(this);
        }
    }
}
