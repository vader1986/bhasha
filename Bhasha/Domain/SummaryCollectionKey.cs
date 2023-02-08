namespace Bhasha.Domain;

public record SummaryCollectionKey(int Level, LangKey LangId)
{
	public static SummaryCollectionKey Parse(string value)
	{
        var args = value.Split('-');

        if (args.Length != 2)
        {
            throw new ArgumentException($"Invalid representation of SummaryCollectionKey: {value}");
        }

        if (!int.TryParse(args[0], out var level))
        {
            throw new ArgumentException($"Invalid level specified for SummaryCollectionKey: {value}");
        }

        return new SummaryCollectionKey(level, LangKey.Parse(args[1]));
    }

    public override string ToString()
    {
        return $"{Level}-{LangId}";
    }

    public static implicit operator string(SummaryCollectionKey key)
    {
        return key.ToString();
    }
}

