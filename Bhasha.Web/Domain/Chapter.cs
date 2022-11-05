namespace Bhasha.Web.Domain;

public record Chapter(
	Guid Id,
	int RequiredLevel,
	Guid NameId,
	Guid DescriptionId,
	Page[] Pages,
	string? ResourceId,
	string AuthorId);