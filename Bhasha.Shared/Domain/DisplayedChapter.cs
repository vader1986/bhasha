namespace Bhasha.Shared.Domain;

public record DisplayedChapter(
    int Id,
    string Name,
    string Description,
    DisplayedPage[] Pages,
    string? ResourceId);

