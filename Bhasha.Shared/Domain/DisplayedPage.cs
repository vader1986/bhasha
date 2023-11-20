namespace Bhasha.Shared.Domain;

public record DisplayedPage(
	PageType PageType,
	Translation Word,
	string? Lead);

public record DisplayedPage<T>(
	PageType PageType,
	Translation Word,
	string? Lead,
	T Arguments) : DisplayedPage(PageType, Word, Lead);
