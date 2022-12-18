namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record DisplayedPage(
	PageType PageType,
	Translation Word,
	string? Lead,
	bool HasLead);

[GenerateSerializer]
public record DisplayedPage<T>(
	PageType PageType,
	Translation Word,
	string? Lead,
	bool HasLead,
	T Arguments) : DisplayedPage(PageType, Word, Lead, HasLead);
