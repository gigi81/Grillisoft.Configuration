using Grillisoft.Configuration.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Grillisoft.Configuration.Xml
{
    internal static class XmlValueStoreReader
    {
        public static async Task<MemoryValuesStore> Load(FileInfo file)
        {
            using (var stream = file.OpenRead())
            using (var reader = XmlReader.Create(stream, Settings))
            {
                return await LoadInternal(reader, file.Directory.FullName);
            }
        }

        private static async Task<MemoryValuesStore> LoadInternal(XmlReader reader, string folder)
        {
            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!reader.Name.Equals("keys", StringComparison.InvariantCultureIgnoreCase))
                            break;

                        var parent = await LoadParent(folder, reader.GetAttribute("parent"));
                        var values = await LoadKeys(reader.ReadSubtree());

                        return new MemoryValuesStore(values, parent);
                }
            }

            throw new Exception("Root 'keys' element not found");
        }

        private static async Task<MemoryValuesStore> LoadParent(string folder, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            return await Load(new FileInfo(Path.Combine(folder, name + ".xml")));
        }

        private static async Task<Dictionary<string, string>> LoadKeys(XmlReader reader)
        {
            var values = new Dictionary<string, string>();
            string key = null;
            string value = null;

            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name.Equals("keys", StringComparison.InvariantCultureIgnoreCase))
                            break;

                        if (!reader.Name.Equals("key", StringComparison.InvariantCultureIgnoreCase))
                            throw new Exception($"Invalid element '{reader.Name}'");

                        key = reader.GetAttribute("name");
                        break;

                    case XmlNodeType.Text:
                        value = await reader.GetValueAsync();
                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Name.Equals("keys", StringComparison.InvariantCultureIgnoreCase))
                            break;

                        if (!reader.Name.Equals("key", StringComparison.InvariantCultureIgnoreCase))
                            throw new Exception($"Invalid element '{reader.Name}'");

                        values.Add(key, value);
                        key = null;
                        value = null;
                        break;
                }
            }

            return values;
        }

        private static string GetPosition(XmlReader reader)
        {
            if (!(reader is IXmlLineInfo info) || !info.HasLineInfo())
                return "(unknown)";

            return "(" + info.LineNumber.ToString() + "," + info.LinePosition.ToString() + ")";
        }

        internal static readonly XmlReaderSettings Settings = new XmlReaderSettings
        {
            Async = true
        };
    }
}
