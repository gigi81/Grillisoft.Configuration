using System.IO;
using System.Threading.Tasks;
using Grillisoft.Configuration.Stores;

namespace Grillisoft.Configuration.Xml
{
    public class XmlValueStore : IValuesStore
    {
        private readonly MemoryValuesStore _store;

        public XmlValueStore()
            : this(new MemoryValuesStore())
        {
        }

        private XmlValueStore(MemoryValuesStore store)
        {
            _store = store;
        }

        public bool IsSafe => false;

        public bool TryGetValue(string key, out string value)
        {
            return _store.TryGetValue(key, out value);
        }

        public static async Task<XmlValueStore> Load(string filename)
        {
            return await Load(new FileInfo(filename));
        }

        public static async Task<XmlValueStore> Load(FileInfo file)
        {
            return new XmlValueStore(await XmlValueStoreReader.Load(file));
        }
    }
}
