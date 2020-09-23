using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Xunit;

namespace Grillisoft.Configuration.Xml.Tests
{
    public class XmlValueStoreReaderTests
    {
        private readonly IValueStoreReader _reader;

        public XmlValueStoreReaderTests()
        {
            _reader = new XmlValueStoreReader();
        }

        [Fact]
        public void LoadRoot()
        {
            var store = _reader.Load(Path.Combine(AssemblyDirectory, "Data"), "Root").Result;

            Assert.Equal("example value", store.Get("key01"));
            Assert.Equal("example value", store.Get("key02"));
            Assert.Equal("example value", store.Get("key03"));
            Assert.Equal("example value", store.Get("key04"));

            Assert.True(store.IsRoot);
        }

        [Fact]
        public void LoadChild01()
        {
            var store = _reader.Load(Path.Combine(AssemblyDirectory, "Data"), "Child01").Result;

            Assert.Equal("example value override", store.Get("key01"));
            Assert.Equal("example value", store.Get("key02"));
            Assert.Equal("example value", store.Get("key03"));
            Assert.Equal("example value", store.Get("key04"));

            Assert.False(store.IsRoot);
        }

        [Fact]
        public void LoadChild02_WithParse()
        {
            var store = _reader.Load(Path.Combine(AssemblyDirectory, "Data"), "Child02_WithParse").Result;

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
