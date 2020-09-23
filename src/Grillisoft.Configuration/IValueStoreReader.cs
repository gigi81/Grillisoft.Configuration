using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grillisoft.Configuration
{
    public interface IValueStoreReader
    {
        /// <summary>
        /// Loads an <see cref="IValueStore"/> from the specified <see cref="folder"/>, using the specified sequence of <see cref="keys"/> until one is found to exist
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IValueStore> Load(string folder, IEnumerable<string> keys);
    }
}
