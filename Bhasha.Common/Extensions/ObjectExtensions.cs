using System.Collections.Generic;
using System.Text.Json;

namespace Bhasha.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<T> ToEnumeration<T>(this T obj)
        {
            return new[] { obj };
        }

        public static string Stringify(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
