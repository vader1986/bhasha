namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record DisplayedPage(
	PageType PageType,
	Translation Word,
	string? Lead);

[GenerateSerializer]
public record DisplayedPage<T>(
	PageType PageType,
	Translation Word,
	string? Lead,
	T Arguments) : DisplayedPage(PageType, Word, Lead);
