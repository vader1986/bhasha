namespace Bhasha.Domain;

public sealed record StudyCard(
    int Id,
    string Language,
    string StudyLanguage,
    string Name,
    string Content,
    string? AudioId);