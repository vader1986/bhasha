namespace Bhasha.Web.Domain;

public record ChapterDescription(
	Guid ChapterId,
	string Name,
	string Description,
	bool Completed);

