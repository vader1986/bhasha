using System.Text;

namespace Bhasha.Infrastructure.EntityFramework.Extensions;

public static class CompactifyExtensions
{
    public static string Compactify<T>(this T[] source, IFormatProvider? formatProvider = default) where T : IConvertible
    {
        return Encoding.UTF8
            .GetString(source
                .Select(x => (byte)(x.ToByte(formatProvider) + 1))
                .ToArray());
    }
    
    public static T[] Decompactify<T>(this string source, Func<byte, T> converter) where T : IConvertible
    {
        return Encoding.UTF8
            .GetBytes(source)
            .Select(b => (byte)(b - 1))
            .Select(converter)
            .ToArray();
    }
}