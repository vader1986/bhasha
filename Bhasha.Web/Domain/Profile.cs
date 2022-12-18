namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record Profile(
	Guid Id,
	ProfileKey Key,
    int Level,
    Guid[] CompletedChapters,
    ChapterSelection? CurrentChapter)
{
    public static Profile Empty(ProfileKey key)
    {
        return new Profile(
            Id: Guid.Empty,
            Key: key,
            Level: 1,
            CompletedChapters: Array.Empty<Guid>(),
            CurrentChapter: null);
    }
}
