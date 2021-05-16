using System;
using System.Collections.Generic;
using System.Linq;

namespace Bhasha.Common.Extensions
{
    public static class RandomExtensions
    {
        public const string Chars = "AaBbCcDdEeFfGgHhIuJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

        /// <summary>
        /// Creates a random string of upper- and lower-case letters.
        /// </summary>
        /// <param name="random">Random number generator</param>
        /// <param name="length">Length of the string (default: -1, random length between 1 and 10)</param>
        /// <returns>A random string consisting only of lower- and upper-case letter.</returns>
        /// <remarks>
        /// Thanks @dtb and @Liam on stackoverflow.
        /// https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        /// </remarks>
        public static string NextString(this Random random, int length = -1)
        {
            length = length != -1 ? length : random.Next(1, 10);

            return new string(
                Enumerable
                    .Repeat(Chars, length)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());
        }

        /// <summary>
        /// Creates an <see cref="IEnumerable{String}"/> of random strings with
        /// the specified settings.
        /// </summary>
        /// <param name="random">Random number generator</param>
        /// <param name="min">Minimum number of strings to create</param>
        /// <param name="max">Maximum number of strings to create</param>
        /// <param name="length">Length of each string</param>
        /// <returns><see cref="IEnumerable{String}"/> of random strings.</returns>
        public static IEnumerable<string> NextStrings(this Random random, int min = 1, int max = 10, int length = -1)
        {
            return Enumerable
                .Range(0, random.Next(min, max))
                .Select(_ => random.NextString(length));
        }

        /// <summary>
        /// A phase of random, whitespace separated strings.
        /// </summary>
        /// <param name="random">Random number generator</param>
        /// <param name="min">Minimum number of words in the phrase</param>
        /// <param name="max">Maximum number of words in the phrase</param>
        /// <param name="length">Length of each string</param>
        /// <returns>A phase of random, whitespace separated strings.</returns>
        public static string NextPhrase(this Random random, int min = 1, int max = 10, int length = -1)
        {
            return string.Join(" ", random.NextStrings(min, max, length));
        }

        /// <summary>
        /// Picks a random elements from the specified elements.
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="random">Random number generator</param>
        /// <param name="elements">Non-empty array of elements to chooses from</param>
        /// <returns>A randomly choosen element from the specified elements.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Array of elements is empty</exception>
        public static T Choose<T>(this Random random, params T[] elements)
        {
            if (elements.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(elements));
            }

            return elements[random.Next(0, elements.Length - 1)];
        }
    }
}
