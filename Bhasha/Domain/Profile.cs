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
            Id: 0,
            Key: key,
            Level: 1,
            CompletedChapters: [],
            CurrentChapter: null);
    }

    public override string ToString()
    {
        return $"{Language.Parse(Key.Native).Pretty()} - {Language.Parse(Key.Target).Pretty()}";
    }
}
