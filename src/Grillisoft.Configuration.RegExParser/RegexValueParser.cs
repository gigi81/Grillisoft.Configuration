using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration.Parsers
{
    public class RegexValueParser : IValueParser
    {
        /// <summary>
        /// Default <see cref="RegexValueParser"/> that will replace values that match ${key}
        /// </summary>
        public static readonly RegexValueParser Default = new RegexValueParser(@"\$\{([^}]+)\}");

        private readonly Regex _regex;

        public RegexValueParser(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public string Parse(string key, string value, IConfigurationRoot configuration)
        {
            return this.ParseInternal(value, configuration, new Stack<string>(new[] { key }));
        }

        private string ParseInternal(string value, IConfigurationRoot configuration, Stack<string> stack)
        {
            if (String.IsNullOrWhiteSpace(value))
                return value;

            return _regex.Replace(value, delegate (Match match) {
                var key = match.Groups[1].Value;
                if (stack.Contains(key))
                    throw new Exception($"Circular call for key {key}"); //TODO: improve

                stack.Push(key);
                return this.ParseInternal(configuration[key], configuration, stack);
            });
        }
    }
}
