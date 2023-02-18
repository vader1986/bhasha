namespace Bhasha.Domain;

[GenerateSerializer]
public record DisplayedSummary(
    Guid ChapterId,
    string Name,
    string Description,
    bool Completed) : Summary(ChapterId, Name, Description);
