using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration
{
    public interface IConfigurationRootWithParser : IConfigurationRoot
    {
        public IValueParser Parser { get; }
    }
}
