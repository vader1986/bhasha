namespace Bhasha.Web.Domain;

public record LangKey(string Native, string Target)
{
    public static LangKey Unknown => new(Language.Unknown, Language.Unknown);

    public static LangKey From(string value)
    {
        var args = value.Split('>');

        if (args == null || args.Length != 2)
        {
            throw new ArgumentException($"Invalid representation of LangKey: {value}");
        }

        if (!Language.Supported.ContainsKey(args[0]))
        {
            throw new ArgumentException($"Native language not supported: {args[0]}");
        }

        if (!Language.Supported.ContainsKey(args[1]))
        {
            throw new ArgumentException($"Target language not supported: {args[0]}");
        }

        return new LangKey(args[0], args[1]);
    }

    public override string ToString()
    {
        return $"{Native}>{Target}";
    }
}

