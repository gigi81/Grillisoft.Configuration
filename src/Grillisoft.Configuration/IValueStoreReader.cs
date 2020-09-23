using System.Threading.Tasks;

namespace Grillisoft.Configuration
{
    public interface IValueStoreReader
    {
        Task<IValueStore> Load(string folder, string name);
    }
}
