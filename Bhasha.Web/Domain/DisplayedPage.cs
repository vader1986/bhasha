namespace Bhasha.Web.Domain;

public record DisplayedPage(
	PageType PageType,
	Translation Word,
	string? Lead,
	bool HasLead);

public record DisplayedPage<T>(
	PageType PageType,
	Translation Word,
	string? Lead,
	bool HasLead,
	T Arguments) : DisplayedPage(PageType, Word, Lead, HasLead);
