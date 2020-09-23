namespace Grillisoft.Configuration
{
    public interface IValueStoreWritable : IValueStore
    {
        void Set(string key, string value);

        void Save();
    }
}
