using CsvHelper.Configuration;

namespace Grillisoft.Configuration.Dolt
{
    public class KeyValueRowMap : ClassMap<KeyValueRow>
    {
        public KeyValueRowMap()
        {
            Map(m => m.Key).Name("key");
            Map(m => m.Value).Name("value");
        }
    }
}
