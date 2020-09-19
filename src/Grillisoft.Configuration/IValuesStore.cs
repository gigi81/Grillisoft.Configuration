namespace Grillisoft.Configuration
{
    public interface IValuesStore
    {
        /// <summary>
        /// Tries to find a <paramref name="value"/> for the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="value">Value found or null if not found</param>
        /// <returns>True if key was found, otherwise false</returns>
        bool TryGetValue(string key, out string value);

        /// <summary>
        /// Returns true if the <see cref="IValuesStore"/> detects circular calls when parsing values on calls to <see cref="TryGetValue"/>, otherwise false
        /// </summary>
        bool IsSafe { get; }
    }
}
