namespace Bhasha.Domain;

public sealed record DisplayedPage(
	Translation Word, 
	string? Lead,
	string? StudyCard);

