using System.Text;

namespace Bhasha.Infrastructure.EntityFramework.Extensions;

public static class ArrayExtensions
{
    public static string Compactify<T>(this T[] source, IFormatProvider? formatProvider = default) where T : IConvertible
    {
        return Encoding.UTF8
            .GetString(source
                .Select(x => x.ToByte(formatProvider))
                .ToArray());
    }
}