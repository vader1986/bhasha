using Generator.Equals;

namespace Bhasha.Shared.Domain;

[Equatable]
public partial record Profile(
	Guid Id,
	ProfileKey Key,
    int Level,
    [property: OrderedEquality]
    Guid[] CompletedChapters,
    ChapterSelection? CurrentChapter)
{
    public static Profile Create(ProfileKey key)
    {
        return new Profile(
            Id: Guid.Empty,
            Key: key,
            Level: 1,
            CompletedChapters: Array.Empty<Guid>(),
            CurrentChapter: null);
    }
}
