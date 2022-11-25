namespace Bhasha.Web.Domain;

public record Profile(
	Guid Id,
	ProfileKey Key,
    int Level,
	Guid? ChapterId,
	Guid[] CompletedChapters,
	int PageIndex,
	ValidationResultType[] Pages);