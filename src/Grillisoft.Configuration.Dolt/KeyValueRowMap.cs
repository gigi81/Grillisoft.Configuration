using CsvHelper.Configuration;

namespace Grillisoft.Configuration
{
    internal class KeyValueRowMap : ClassMap<KeyValueRow>
    {
        public KeyValueRowMap(string keyField, string valueField)
        {
            Map(m => m.Key).Name(keyField);
            Map(m => m.Value).Name(valueField);
        }
    }
}
