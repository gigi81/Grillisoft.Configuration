using Microsoft.Extensions.Configuration;
using Xunit;

namespace Grillisoft.Configuration.Xml.Tests
{
    public class XmlKeyConfigurationSourceTests
    {
        [Fact]
        public void LoadRoot()
        {
            var configuration = new ConfigurationBuilder()
                .AddXmlTree("Data", new[] { "root" })
                .Build();

            Assert.Equal("example value", configuration["key01"]);
            Assert.Equal("example value", configuration["key02"]);
            Assert.Equal("example value", configuration["key03"]);
            Assert.Equal("example value", configuration["key04"]);
        }

        [Fact]
        public void LoadChild01()
        {
            var configuration = new ConfigurationBuilder()
                .AddXmlTree("Data", new[] { "child01" })
                .Build();

            Assert.Equal("example value override", configuration["key01"]);
            Assert.Equal("example value", configuration["key02"]);
            Assert.Equal("example value", configuration["key03"]);
            Assert.Equal("example value", configuration["key04"]);
        }

        [Fact]
        public void LoadNullDirectory()
        {
            var configuration = new ConfigurationBuilder()
                .AddXmlTree(new[] { "nodir" })
                .Build();

            Assert.Equal("example value nodir", configuration["key01"]);
        }
    }
}
