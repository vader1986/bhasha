namespace Bhasha.Web.Domain;

public record ChapterKey(Guid ChapterId, LangKey LangId)
{
    public static ChapterKey From(string value)
    {
        var args = value.Split('-');

        if (args == null || args.Length != 2)
        {
            throw new ArgumentException($"Invalid representation of ChapterKey: {value}");
        }

        if (!Guid.TryParse(args[0], out var chapterId))
        {
            throw new ArgumentException($"Invalid representation of ChapterKey: {value}");
        }

        return new ChapterKey(chapterId, LangKey.From(args[1]));
    }

    public override string ToString()
    {
        return $"{ChapterId}-{LangId}";
    }
}
