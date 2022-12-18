namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record ChapterKey(Guid ChapterId, LangKey LangId)
{
    public const string Separator = ":";

    public static ChapterKey Parse(string value)
    {
        var args = value.Split(Separator);

        if (args.Length != 2)
        {
            throw new ArgumentException($"Invalid representation of ChapterKey: {value}");
        }

        if (!Guid.TryParse(args[0], out var chapterId))
        {
            throw new ArgumentException($"Invalid representation of ChapterKey: {value}");
        }

        return new ChapterKey(chapterId, LangKey.Parse(args[1]));
    }

    public override string ToString()
    {
        return $"{ChapterId}{Separator}{LangId}";
    }
}
