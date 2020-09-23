using System.Collections.Generic;

namespace Grillisoft.Configuration.Stores
{
    public class SafeValueStore : IValueStore
    {
        private readonly HashSet<string> _keys = new HashSet<string>();
        private readonly IValueStore _store;

        public SafeValueStore(IValueStore store)
        {
            _store = store;
        }

        public SafeValueStore(IValueStore store, string key)
            : this(store)
        {
            _keys.Add(key);
        }

        public bool IsSafe => true;

        public bool IsRoot => _store.IsRoot;

        public bool TryGetValue(string key, out string value)
        {
            _keys.Add(key);

            try
            {
                return _store.TryGetValue(key, out value);
            }
            finally
            {
                _keys.Remove(key);
            }
        }

        public void ClearStack()
        {
            _keys.Clear();
        }
    }
}
