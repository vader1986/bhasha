namespace Bhasha.Shared.Domain;

public record DisplayedSummary(
    Guid ChapterId,
    string Name,
    string Description,
    bool Completed) : Summary(ChapterId, Name, Description);
