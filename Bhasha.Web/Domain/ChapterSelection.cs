namespace Bhasha.Web.Domain;

public record ChapterSelection(
    Guid ChapterId,
    int PageIndex,
    ValidationResultType[] Pages);

