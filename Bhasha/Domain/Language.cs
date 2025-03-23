﻿namespace Bhasha.Domain;

using LanguageSet = Dictionary<string, Language>;

public class Language(string id, string name, string? region = null) : IEquatable<Language>
{
    public static readonly Language Unknown = new (string.Empty, string.Empty);
    public static readonly Language English = new ("en", "English", "UK");
    public static readonly Language Bengali = new ("bn", "Bengali");
    public static readonly Language Reference = English;

    public static readonly LanguageSet Supported = new()
    {
        [English.ToString()] = English,
        [Bengali.ToString()] = Bengali
    };

    public string Id { get; } = id;
    public string Name { get; } = name;
    public string? Region { get; } = region;

    public static Language Parse(string? tag)
    {
        if (tag != null && Supported.TryGetValue(tag, out var language))
        {
            return language;
        }

        return Unknown;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        return obj is Language other && Id == other.Id && Region == other.Region;
    }

    public bool Equals(Language? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Region);
    }

    public static bool operator !=(Language? lhs, Language? rhs)
    {
        return !Equals(lhs, rhs);
    }

    public static bool operator ==(Language? lhs, Language? rhs)
    {
        return Equals(lhs, rhs);
    }

    public override string ToString()
    {
        return Region != null ? $"{Id}_{Region}" : Id;
    }

    public string Pretty()
    {
        return Region != null ? $"{Name} ({Region})" : Name;
    }

    public static implicit operator string(Language language)
    {
        return language.ToString();
    }

    public static implicit operator Language(string language)
    {
        return Parse(language);
    }
}

