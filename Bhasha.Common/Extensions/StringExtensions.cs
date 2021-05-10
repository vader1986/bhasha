using System.Collections.Generic;
using System.Linq;

namespace Bhasha.Common.Extensions
{
    public static class StringExtensions
    {
        private static readonly ISet<char> Signs = new HashSet<char> {
            '.', '?', '!', ',', '-', ';'
        };

        public static string ToUpperFirstLetter(this string str)
        {
            if (str.Length == 0) return str;
            if (str.Length == 1) return str.ToUpperInvariant();

            return char.ToUpperInvariant(str[0]) + str[1..];
        }

        public static bool StartsWithSign(this string str)
        {
            return !string.IsNullOrWhiteSpace(str) && Signs.Contains(str[0]);
        }

        public static bool EndsWithSign(this string str)
        {
            return !string.IsNullOrWhiteSpace(str) && Signs.Contains(str.Last());
        }
    }
}
