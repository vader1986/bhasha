using System;
using System.Collections.Generic;
using System.Linq;

namespace Bhasha.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> source)
        {
            var items = source.ToArray();

            if (items.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }

            var rnd = new Random();
            var index = rnd.NextDouble() * items.Length;

            return items[(int)index];
        }

        public static T? RandomOrDefault<T>(this IEnumerable<T> source) where T : class
        {
            var items = source.ToArray();

            if (items.Length == 0)
            {
                return default;
            }

            var rnd = new Random();
            var index = rnd.NextDouble() * items.Length;

            return items[(int)index];
        }

        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }
    }
}
