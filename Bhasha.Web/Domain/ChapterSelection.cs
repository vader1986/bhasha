namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record ChapterSelection(
    Guid ChapterId,
    int PageIndex,
    ValidationResult[] Pages);

