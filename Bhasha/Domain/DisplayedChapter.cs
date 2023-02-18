namespace Bhasha.Domain;

[GenerateSerializer]
public record DisplayedChapter(
    Guid Id,
    string Name,
    string Description,
    DisplayedPage[] Pages,
    string? ResourceId);

