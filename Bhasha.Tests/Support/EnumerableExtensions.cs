using System.Collections.Generic;
using System.Linq;

namespace Bhasha.Tests.Support;

public static class EnumerableExtensions
{
    public static IEnumerable<T> TakeEverySecond<T>(this IEnumerable<T> source, int offset = 0)
    {
        return source.Where((_, i) => (i + offset) % 2 == 1);
    }
}