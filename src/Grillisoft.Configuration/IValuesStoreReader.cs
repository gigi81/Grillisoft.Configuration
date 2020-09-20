using System.Threading.Tasks;

namespace Grillisoft.Configuration
{
    public interface IValuesStoreReader
    {
        Task<IValuesStore> Load(string folder, string name);
    }
}
