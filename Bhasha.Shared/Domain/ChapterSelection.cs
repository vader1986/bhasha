using Generator.Equals;

namespace Bhasha.Shared.Domain;

[Equatable]
public partial record ChapterSelection(
    int ChapterId,
    int PageIndex,
    [property:OrderedEquality]
    ValidationResult[] Pages);

