using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration
{
    public interface IConfigurationSectionWithParser : IConfigurationSection
    {
        public IValueParser Parser { get; }
    }
}
