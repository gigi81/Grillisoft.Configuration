using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Grillisoft.Configuration.Xml
{
    public class XmlTreeConfigurationProvider : FileTreeConfigurationProvider<XmlTreeConfigurationSource>
    {
        public XmlTreeConfigurationProvider(XmlTreeConfigurationSource source) : base(source) {}

        public override IDictionary<string, string> Load(Stream stream, out string parent)
        {
            using (var reader = XmlReader.Create(stream, Settings))
            {
                return LoadKeys(reader, out parent);
            }
        }

        private static IDictionary<string, string> LoadKeys(XmlReader reader, out string parent)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!reader.Name.Equals("keys", StringComparison.InvariantCultureIgnoreCase))
                            break;

                        parent = reader.GetAttribute("parent");
                        return LoadKeysSubTree(reader.ReadSubtree());
                }
            }

            throw new Exception("Root 'keys' element not found");
        }

        private static Dictionary<string, string> LoadKeysSubTree(XmlReader reader)
        {
            var values = new Dictionary<string, string>();
            string key = null;
            string value = null;

            while (reader.Read())
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
                        value = reader.Value;
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
            Async = false
        };
    }
}
