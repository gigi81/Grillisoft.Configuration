using System.Threading.Tasks;

namespace Grillisoft.Configuration
{
    public static class IValueStoreReaderExtensions
    {
        public async static Task<IValueStore> Load(this IValueStoreReader reader, string folder, params string[] keys)
        {
            return await reader.Load(folder, keys);
        }
    }
}
