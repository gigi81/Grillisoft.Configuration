using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration
{
    public class DoltTreeConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Url to the dolt repo
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Branch to clone
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Name of the field in the dolt table used as a key for the configuration values (default "key")
        /// </summary>
        public string KeyField { get; set; } = "key";

        /// <summary>
        /// Name of the filed in the dolt table to use as configuration values (default "value")
        /// </summary>
        public string ValueField { get; set; } = "value";

        /// <summary>
        /// Name of the key in the table to be used to identify the parent for the current table (default ".parent")
        /// </summary>
        public string ParentKey { get; set; } = ".parent";

        /// <summary>
        /// Keys used to find the configuration to load
        /// </summary>
        public virtual IEnumerable<string> Keys { get; set; }

        /// <summary>
        /// Builds the <see cref="IConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="IConfigurationProvider"/></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DoltTreeConfigurationProvider(this);
        }
    }
}
