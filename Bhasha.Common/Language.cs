#nullable enable
using System;

namespace Bhasha.Common
{
    public class Language : IEquatable<Language>
    {
        public string Id { get; }
        public string? Region { get; }

        private Language(string id, string? region = default)
        {
            Id = id;
            Region = region;
        }

        public static Language Parse(string tag)
        {
            var parts = tag.Split("_");
            return new Language(parts[0], parts.Length > 1 ? parts[1] : default);
        }

        public bool Equals(Language other)
        {
            if (other == null) return false;
            return Id == other.Id && Region == other.Region;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Region);
        }
    }
}
