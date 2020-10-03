using Grillisoft.Configuration.Parsers;
using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration.RegExParser
{
    public static class RegexValueParserExtensions
    {
        /// <summary>
        /// Adds a parser to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="regex">Regular expression to extract substitution keys</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddRegExParser(this IConfigurationBuilder builder, string regex = null)
        {
            if (builder is IConfigurationBuilderWithParser builderWithParser)
                return builderWithParser.AddRegExParser(regex);

            return new ConfigurationBuilderWithParser(builder).AddRegExParser(regex);
        }

        /// <summary>
        /// Adds a parser to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="regex">Regular expression to extract substitution keys</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddRegExParser(this IConfigurationBuilderWithParser builder, string regex = null)
        {
            builder.Parser = string.IsNullOrEmpty(regex) ? RegexValueParser.Default : new RegexValueParser(regex);
            return builder;
        }
    }
}
