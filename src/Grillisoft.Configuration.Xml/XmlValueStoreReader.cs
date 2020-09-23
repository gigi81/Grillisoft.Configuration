using Grillisoft.Configuration.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Grillisoft.Configuration.Xml
{
    public class XmlValueStoreReader : IValueStoreReader
    {
        public async Task<IValueStore> Load(string folder, string name)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/using-async-for-file-access
            using (var stream = new FileStream(Path.Combine(folder, name + ".xml"), FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            using (var reader = XmlReader.Create(stream, Settings))
            {
                var data = await LoadInternal(reader, folder);
                var parent = await LoadParent(folder, data.Item2);

                return new MemoryValueStore(data.Item1, parent);
            }
        }

        private async Task<(IDictionary<string, string>, string)> LoadInternal(XmlReader reader, string folder)
        {
            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!reader.Name.Equals("keys", StringComparison.InvariantCultureIgnoreCase))
                            break;

                        var parent = reader.GetAttribute("parent");
                        var values = await LoadKeys(reader.ReadSubtree());

                        return (values, parent);
                }
            }

            throw new Exception("Root 'keys' element not found");
        }

        private async Task<IValueStore> LoadParent(string folder, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            return await this.Load(folder, name);
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
