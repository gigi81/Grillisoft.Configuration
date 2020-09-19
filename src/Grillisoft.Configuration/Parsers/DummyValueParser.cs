namespace Grillisoft.Configuration.Parsers
{
    public class DummyValueParser : IValueParser
    {
        public string Parse(string key, string value, IValuesStore valuesStore)
        {
            return value;
        }
    }
}
