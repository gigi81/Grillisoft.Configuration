using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Grillisoft.Configuration.Xml.Tests
{
    public class XmlValueStoreTests
    {
        [Fact]
        public void LoadRoot()
        {
            var root = XmlValueStore.Load(Path.Combine(AssemblyDirectory, "Data", "Root.xml")).Result;

            Assert.Equal("example value", root.Get("key01"));
            Assert.Equal("example value", root.Get("key02"));
            Assert.Equal("example value", root.Get("key03"));
            Assert.Equal("example value", root.Get("key04"));
        }

        [Fact]
        public void LoadChild01()
        {
            var root = XmlValueStore.Load(Path.Combine(AssemblyDirectory, "Data", "Child01.xml")).Result;

            Assert.Equal("example value override", root.Get("key01"));
            Assert.Equal("example value", root.Get("key02"));
            Assert.Equal("example value", root.Get("key03"));
            Assert.Equal("example value", root.Get("key04"));
        }

        [Fact]
        public void LoadChild02_WithParse()
        {
            var root = XmlValueStore.Load(Path.Combine(AssemblyDirectory, "Data", "Child02_WithParse.xml")).Result;

            Assert.Equal("example value override", root.Get("key01"));
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
