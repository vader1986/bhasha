using System;
using System.Collections.Generic;
using System.Linq;

namespace Bhasha.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void Shuffle<T>(this T[] source)
        {
            var rnd = new Random();
            var n = source.Length;

            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = source[k];
                source[k] = source[n];
                source[n] = value;
            }
        }

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

        public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int number)
        {
            var remaining = source.ToList();
            var chosen = new List<T>(number);

            while (chosen.Count < number && !remaining.IsEmpty())
            {
                var item = remaining.Random();
                remaining.Remove(item);
                chosen.Add(item);
            }

            return chosen;
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

        public static int IndexOf<T>(this IEnumerable<T> source, T obj)
        {
            var array = source.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (Equals(array[i], obj))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
