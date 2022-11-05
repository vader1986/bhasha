namespace Bhasha.Web.Domain;

public record ValidationInput(
	LangKey Languages,
	Guid ExpressionId,
	Translation Translation);

