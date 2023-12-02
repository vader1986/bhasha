using System.Text;

namespace Bhasha.Infrastructure.EntityFramework.Extensions;

public static class StringExtensions
{
    public static T[] Decompactify<T>(this string source, Func<byte, T> converter)
    {
        return Encoding.UTF8
            .GetBytes(source)
            .Select(converter)
            .ToArray();
    }
}