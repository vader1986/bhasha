using System.Collections.Generic;

namespace Bhasha.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<T> ToEnumeration<T>(this T obj)
        {
            return new[] { obj };
        }
    }
}
