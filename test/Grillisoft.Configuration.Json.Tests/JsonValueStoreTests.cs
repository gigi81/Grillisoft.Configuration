using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Grillisoft.Configuration.Json.Tests
{
    public class JsonValueStoreTests
    {
        [Fact]
        public void LoadRoot()
        {
            var root = JsonValueStore.Load(Path.Combine(AssemblyDirectory, "Data", "root.json")).Result;

            Assert.Equal("example value", root.Get("key01"));
            Assert.Equal("example value", root.Get("key02"));
            Assert.Equal("example value", root.Get("key03"));
            Assert.Equal("example value", root.Get("key04"));
        }

        private static string AssemblyDirectory
        {
            get
            {
                UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
