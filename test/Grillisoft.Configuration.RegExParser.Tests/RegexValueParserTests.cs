using Microsoft.Extensions.Configuration;
using Xunit;

namespace Grillisoft.Configuration.RegExParser.Tests
{
    public class RegexValueParserTests
    {
        [Fact]
        public void RootKeysParsing()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddRegExParser()
                .Build();

            // "MyKey": "My Value ${MyKey2}",
            // "MyKey2": "with a variable",

            Assert.Equal("My Value with a variable", configuration["MyKey"]);
            Assert.Equal("with a variable", configuration["MyKey2"]);
        }

        [Fact]
        public void SectionKeysParsing()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddRegExParser()
                .Build();

            // "MyKey": "My Value ${MyKey2}",
            // "MyKey2": "with a variable",

            var section = configuration.GetSection("Logging").GetSection("LogLevel");

            Assert.Equal("Error", configuration["MyKey3"]);
            Assert.Equal("Error", section["Default"]);
        }
    }
}