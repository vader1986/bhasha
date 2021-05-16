using System.Text.Json;

namespace Bhasha.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static string Stringify(this object obj)
        {
            if (obj == null) return "null";
            return JsonSerializer.Serialize(obj);
        }
    }
}
