namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record Summary(
    Guid ChapterId,
    string Name,
    string Description);

