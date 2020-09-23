using System.Collections.Generic;
using Grillisoft.Configuration.Parsers;

namespace Grillisoft.Configuration.Stores
{
    public class MemoryValuesStore : IValuesStoreWritable
    {
        private readonly Dictionary<string, string> _values;
        private readonly IValueParser _parser;
        private readonly IValuesStore _parent;

        public MemoryValuesStore(IValuesStore parent = null)
            : this(null, parent, null)
        {
        }

        public MemoryValuesStore(IDictionary<string, string> source, IValuesStore parent = null, IValueParser parser = null)
        {
            _values = source != null ? new Dictionary<string, string>(source) : new Dictionary<string, string>();
            _parent = parent;
            _parser = parser ?? RegexValueParser.Default;
        }

        public bool IsSafe => false;

        public bool IsRoot => _parent == null;

        public bool TryGetValue(string key, out string value)
        {
            if (!TryGetValueInternal(key, out value))
                return false;

            value = _parser.Parse(key, value, this);
            return true;
        }

        private bool TryGetValueInternal(string key, out string value)
        {
            if (_values.TryGetValue(key, out value))
                return true;

            if (_parent != null && _parent.TryGetValue(key, out value))
                return true;

            return false;
        }

        public void Set(string key, string value)
        {
            _values[key] = value;
        }

        public void Save()
        {
        }
    }
}
