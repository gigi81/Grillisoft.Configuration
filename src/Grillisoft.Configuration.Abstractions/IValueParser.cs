using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration
{
    public interface IValueParser
    {
        string Parse(string key, string value, IConfigurationRoot configuration);
    }
}
