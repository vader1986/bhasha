namespace Bhasha.Web.Domain;

public record DisplayedChapter(Guid Id, string Name, string Description, DisplayedPage[] Pages, string? ResourceId);

