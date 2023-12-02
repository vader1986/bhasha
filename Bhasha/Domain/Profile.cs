using Generator.Equals;

namespace Bhasha.Domain;

[Equatable]
public partial record Profile(
    int Id,
	ProfileKey Key,
    int Level,
    [property: OrderedEquality]
    int[] CompletedChapters,
    ChapterSelection? CurrentChapter)
{
    public static Profile Create(ProfileKey key)
    {
        return new Profile(
            Id: default,
            Key: key,
            Level: 1,
            CompletedChapters: Array.Empty<int>(),
            CurrentChapter: null);
    }
}
