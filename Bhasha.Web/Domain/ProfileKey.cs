namespace Bhasha.Web.Domain;

public record ProfileKey(string UserId, LangKey LangId)
{
    public const string Separator = ":";

    public static ProfileKey Parse(string value)
    {
        var args = value.Split(Separator);

        if (args.Length != 2)
        {
            throw new ArgumentException($"Invalid representation of ProfileKey: {value}");
        }

        return new ProfileKey(args[0], LangKey.Parse(args[1]));
    }

    public override string ToString()
    {
        return $"{UserId}{Separator}{LangId}";
    }
}

