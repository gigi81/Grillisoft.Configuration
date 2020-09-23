using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grillisoft.Configuration
{
    public interface IValueStoreWriter
    {
        Task Save(IDictionary<string, string> values, string parent, string folder, string key);
    }
}
