using System;
using System.Text.RegularExpressions;

namespace Grillisoft.Configuration
{
    internal class DoltTable
    {
        private static readonly Regex TableInfoRegEx = new Regex(@"([\w]+)[\t\s]+([\w]+)[\t\s]+([\d]+)", RegexOptions.Compiled);

        public static bool IsMatch(string row)
        {
            return TableInfoRegEx.IsMatch(row);
        }

        public DoltTable(string row)
        {
            var match = TableInfoRegEx.Match(row);
            if (!match.Success)
                throw new ArgumentException(nameof(row));

            this.Name = match.Groups[1].Value;
            this.Hash = match.Groups[2].Value;
            this.Rows = long.Parse(match.Groups[3].Value);
        }

        public string Name { get; }

        public string Hash { get; }

        public long Rows { get; }
    }
}
