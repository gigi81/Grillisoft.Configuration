namespace Grillisoft.Configuration.Parsers
{
    public class DummyValueParser : IValueParser
    {
        public string Parse(string key, string value, IValueStore valuesStore)
        {
            return value;
        }
    }
}
