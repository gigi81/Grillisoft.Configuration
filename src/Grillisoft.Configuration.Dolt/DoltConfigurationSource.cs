using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration
{
    public class DoltConfigurationSource : IConfigurationSource
    {
        public string Url { get; set; }

        public string Branch { get; set; }

        /// <summary>
        /// Keys used to find the configuration to load
        /// </summary>
        public virtual IEnumerable<string> Keys { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DoltConfigurationProvider(this);
        }
    }
}
