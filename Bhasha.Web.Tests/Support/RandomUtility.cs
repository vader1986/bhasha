using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;

namespace Bhasha.Web.Tests.Support;

public static class RandomUtility
{
    private static readonly Fixture _rog = new();

    public static T Random<T>(this IEnumerable<T> source)
    {
        var elements = source.ToArray();
        if (elements.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(source));

        var index = Math.Abs(_rog.Create<int>()) % elements.Length;
        return elements[index];
    }

}

