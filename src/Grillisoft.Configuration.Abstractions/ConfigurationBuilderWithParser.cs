using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Grillisoft.Configuration
{
    public class ConfigurationBuilderWithParser : IConfigurationBuilderWithParser
    {
        private readonly IConfigurationBuilder _wrapped;

        public ConfigurationBuilderWithParser(IConfigurationBuilder wrapped)
        {
            _wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }

        public IValueParser Parser { get; set; }

        public IDictionary<string, object> Properties => _wrapped.Properties;

        public IList<IConfigurationSource> Sources => _wrapped.Sources;

        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            return _wrapped.Add(source);
        }

        public IConfigurationRoot Build()
        {
            return new ConfigurationRootWithParser(_wrapped.Build(), this.Parser);
        }
    }
}
