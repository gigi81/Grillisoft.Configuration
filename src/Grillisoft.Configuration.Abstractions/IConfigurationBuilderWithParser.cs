using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration
{
    public interface IConfigurationBuilderWithParser : IConfigurationBuilder
    {
        IValueParser Parser { get; set; }
    }
}
