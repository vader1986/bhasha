namespace Bhasha.Shared.Domain;

public record ChapterKey(Guid ChapterId, ProfileKey ProfileKey)
{
    public override string ToString()
    {
        return $"{ChapterId}:[{ProfileKey}]";
    }
}
