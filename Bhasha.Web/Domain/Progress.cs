namespace Bhasha.Web.Domain;

public record Progress(int Level, Guid ChapterId, Guid[] CompletedChapters, int PageIndex, ValidationResultType[] Pages);