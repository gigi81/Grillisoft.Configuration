using System.IO;
using System.Threading.Tasks;
using Grillisoft.Configuration.Stores;

namespace Grillisoft.Configuration.Json
{
    public class JsonValueStore : IValuesStore
    {
        private readonly MemoryValuesStore _store;

        public JsonValueStore()
            : this(new MemoryValuesStore())
        {
        }

        private JsonValueStore(MemoryValuesStore store)
        {
            _store = store;
        }

        public bool IsSafe => false;

        public bool TryGetValue(string key, out string value)
        {
            return _store.TryGetValue(key, out value);
        }

        public static async Task<JsonValueStore> Load(string filename)
        {
            return await Load(new FileInfo(filename));
        }

        public static async Task<JsonValueStore> Load(FileInfo file)
        {
            return new JsonValueStore(await JsonValueStoreReader.Load(file));
        }
    }
}
