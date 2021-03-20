using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bhasha.Common.Extensions
{
    public static class AssemblyExtensions
    {
        public static IDictionary<Type, T> GetTypesWithAttribute<T>(this Assembly assembly) where T : Attribute
        {
            return assembly
                .GetTypes()
                .Where(t => t.GetCustomAttributes<T>(true).Any())
                .ToDictionary(t => t, t => t.GetCustomAttribute<T>());
        }
    }
}
