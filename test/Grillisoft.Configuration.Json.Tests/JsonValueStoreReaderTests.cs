using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Grillisoft.Configuration.Json.Tests
{
    public class JsonValueStoreReaderTests
    {
        private readonly IValuesStoreReader _reader;

        public JsonValueStoreReaderTests()
        {
            _reader = new JsonValueStoreReader();
        }

        [Fact]
        public void LoadRoot()
        {
            var store = _reader.Load(Path.Combine(AssemblyDirectory, "Data"), "root").Result;

            Assert.Equal("example value", store.Get("key01"));
            Assert.Equal("example value", store.Get("key02"));
            Assert.Equal("example value", store.Get("key03"));
            Assert.Equal("example value", store.Get("key04"));

            Assert.True(store.IsRoot);
        }

        [Fact]
        public void LoadChild01()
        {
            var store = _reader.Load(Path.Combine(AssemblyDirectory, "Data"), "child01").Result;

            Assert.Equal("example value override", store.Get("key01"));
            Assert.Equal("example value", store.Get("key02"));
            Assert.Equal("example value", store.Get("key03"));
            Assert.Equal("example value", store.Get("key04"));

            Assert.False(store.IsRoot);
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
