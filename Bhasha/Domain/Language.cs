namespace Bhasha.Domain;

using LanguageSet = Dictionary<string, Language>;

[GenerateSerializer]
public class Language : IEquatable<Language>
{
    public readonly static Language Unknown = new (string.Empty, string.Empty);
    public readonly static Language English = new ("en", "English", "UK");
    public readonly static Language Bengali = new ("bn", "Bengali");
    public readonly static Language Reference = English;

    public readonly static LanguageSet Supported = new()
    {
        [English.ToString()] = English,
        [Bengali.ToString()] = Bengali
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

    public static Language Parse(string? tag)
    {
        if (tag != null && Supported.TryGetValue(tag, out var language))
        {
            return language;
        }

        return Unknown;
    }

    public bool IsSupported()
    {
        return Supported.ContainsKey(this.ToString());
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
        return Region != default ? $"{Id}_{Region}" : Id;
    }

    public string Pretty()
    {
        return Region != default ? $"{Name} ({Region})" : Name;
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

