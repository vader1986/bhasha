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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj is Language other && Id == other.Id && Region == other.Region;
        }

        public bool Equals(Language other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Region);
        }

        public static bool operator !=(Language lhs, Language rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator ==(Language lhs, Language rhs)
        {
            return lhs.Equals(rhs);
        }
    }
}
