namespace Bhasha.Shared.Domain;

public record DisplayedSummary(
    int ChapterId,
    string Name,
    string Description,
    bool Completed) : Summary(ChapterId, Name, Description);
