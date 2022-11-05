namespace Bhasha.Web.Domain;

public record Profile(
	Guid Id,
	string UserId,
	LangKey Languages,
	Progress Progress);