using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grillisoft.Configuration
{
    public class ConfigurationSectionWithParser : IConfigurationSectionWithParser
    {
        private readonly IConfigurationSection _wrapped;
        private readonly IValueParser _parser;
        private readonly IConfigurationRoot _root;

        public ConfigurationSectionWithParser(IConfigurationSection wrapped, IValueParser parser, IConfigurationRoot root)
        {
            _wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _root = root ?? throw new ArgumentNullException(nameof(root));
        }

        public string this[string key]
        {
            get => _parser.Parse(key, _wrapped[key], _root);
            set => _wrapped[key] = value;
        }

        public IValueParser Parser => _parser;

        public string Key => _wrapped.Key;

        public string Path => _wrapped.Path;

        public string Value
        {
            get => _parser.Parse(this.Key, _wrapped.Value, _root);
            set => _wrapped.Value = value;
        }

        public IChangeToken GetReloadToken()
        {
            return _wrapped.GetReloadToken();
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
                        new ConfigurationSectionWithParser(section, _parser, _root);
        }
    }
}
