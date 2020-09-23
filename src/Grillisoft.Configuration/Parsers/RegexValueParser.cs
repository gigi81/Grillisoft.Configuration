using System;
using System.Text.RegularExpressions;
using Grillisoft.Configuration.Stores;

namespace Grillisoft.Configuration.Parsers
{
    public class RegexValueParser : IValueParser
    {
        private readonly Regex _regex;

        /// <summary>
        /// Default <see cref="RegexValueParser"/> that will replace values that match ${key}
        /// </summary>
        public static readonly RegexValueParser Default = new RegexValueParser(@"\$\{([^}]+)\}");

        public RegexValueParser(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public string Parse(string key, string value, IValueStore store)
        {
            return this.ParseSafe(value, store.IsSafe ? store : new SafeValueStore(store, key));
        }

        private string ParseSafe(string value, IValueStore store)
        {
            if (String.IsNullOrWhiteSpace(value))
                return value;

            return _regex.Replace(value, delegate (Match match) {
                return store.Get(match.Groups[1].Value);
            });
        }
    }
}
