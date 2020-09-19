namespace Grillisoft.Configuration
{
    public interface IValuesStoreWritable : IValuesStore
    {
        void Set(string key, string value);

        void Save();
    }
}
