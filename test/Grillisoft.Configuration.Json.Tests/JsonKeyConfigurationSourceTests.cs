using Microsoft.Extensions.Configuration;
using Xunit;

namespace Grillisoft.Configuration.Json.Tests
{
    public class JsonKeyConfigurationSourceTests
    {
        [Fact]
        public void LoadRoot()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonKeyFile("Data", new[] { "root" })
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
                .AddJsonKeyFile("Data", new[] { "child01" })
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
                .AddJsonKeyFile(new[] { "nodir" })
                .Build();

            Assert.Equal("example value nodir", configuration["key01"]);
        }
    }
}
