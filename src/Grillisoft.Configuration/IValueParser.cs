namespace Grillisoft.Configuration
{
    public interface IValueParser
    {
        string Parse(string key, string value, IValuesStore valuesStore);
    }
}
