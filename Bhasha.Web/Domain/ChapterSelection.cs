using Generator.Equals;

namespace Bhasha.Web.Domain;

[Equatable]
[GenerateSerializer]
public partial record ChapterSelection(
    Guid ChapterId,
    int PageIndex,
    [property:OrderedEquality]
    ValidationResult[] Pages);

