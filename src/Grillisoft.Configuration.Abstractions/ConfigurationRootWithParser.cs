using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grillisoft.Configuration
{
    public class ConfigurationRootWithParser : IConfigurationRootWithParser
    {
        private readonly IConfigurationRoot _wrapped;
        private readonly IValueParser _parser;

        public ConfigurationRootWithParser(IConfigurationRoot wrapped, IValueParser parser)
        {
            _wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public string this[string key]
        {
            get => _parser.Parse(key, _wrapped[key], this);
            set => _wrapped[key] = value;
        }

        public IEnumerable<IConfigurationProvider> Providers => _wrapped.Providers;

        public IValueParser Parser => _parser;

        public IChangeToken GetReloadToken()
        {
            return _wrapped.GetReloadToken();
        }

        public void Reload()
        {
            _wrapped.Reload();
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _wrapped.GetChildren()
                           .Select(GetConfigurtionSectionWithParser);
        }

        public IConfigurationSection GetSection(string key)
        {
            return GetConfigurtionSectionWithParser(_wrapped.GetSection(key));
        }

        private IConfigurationSectionWithParser GetConfigurtionSectionWithParser(IConfigurationSection section)
        {
            if (section == null)
                return null;

            return section is IConfigurationSectionWithParser sectionWithParser ?
                        sectionWithParser :
                        new ConfigurationSectionWithParser(section, _parser, this);
        }
    }
}
