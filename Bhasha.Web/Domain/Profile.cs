using Generator.Equals;

namespace Bhasha.Web.Domain;

[Equatable]
[GenerateSerializer]
public partial record Profile(
	Guid Id,
	ProfileKey Key,
    int Level,
    [property: OrderedEquality]
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
