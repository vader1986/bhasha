using System;
using System.Collections.Generic;

namespace Bhasha.Common
{
    using LanguageSet = Dictionary<string, Language>;

    public class Language : IEquatable<Language>
    {

        public readonly static Language English = new Language("en", "English", "UK");
        public readonly static Language Bengoli = new Language("bn", "Bengoli");

        public readonly static LanguageSet Supported = new LanguageSet
        {
            { English, English }, { Bengoli, Bengoli }
        };

        public string Id { get; }
        public string Name { get; }
        public string? Region { get; }

        public Language(string id, string name, string? region = default)
        {
            Id = id;
            Name = name;
            Region = region;
        }

        public static Language Parse(string tag)
        {
            return Supported[tag];
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

        public override string ToString()
        {
            return Region != default ? $"{Id}_{Region}" : Id;
        }

        public static implicit operator string(Language language)
        {
            return language.ToString();
        }
    }
}
