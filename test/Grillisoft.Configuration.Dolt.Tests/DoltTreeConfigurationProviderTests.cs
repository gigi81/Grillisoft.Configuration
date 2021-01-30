using Xunit;
using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration.Tests
{
    public class DoltTreeConfigurationProviderTests
    {
        [Fact]
        public void LoadUat()
        {
            var configuration = new ConfigurationBuilder()
                .AddDoltTree("gigi81/test-configuration", new[] { "uat" })
                .AddRegExParser()
                .Build();

            Assert.Equal("uat", configuration["env"]);
            Assert.Equal("mail-uat.contoso.com", configuration["mailhost"]);
            Assert.Equal("sql-uat.contoso.com", configuration["sqlserverhost"]);
        }

        [Fact]
        public void LoadDev()
        {
            var configuration = new ConfigurationBuilder()
                .AddDoltTree("gigi81/test-configuration", new[] { "dev" })
                .AddRegExParser()
                .Build();

            Assert.Equal("dev", configuration["env"]);
            Assert.Equal("mail-dev.contoso.com", configuration["mailhost"]);
            Assert.Equal("localhost", configuration["sqlserverhost"]);
        }
    }
}
