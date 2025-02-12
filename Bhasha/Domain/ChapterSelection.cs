using Generator.Equals;

namespace Bhasha.Domain;

[Equatable]
public partial record ChapterSelection(
    int ChapterId,
    int PageIndex,
    [property:OrderedEquality]
    byte[] CorrectAnswers);

