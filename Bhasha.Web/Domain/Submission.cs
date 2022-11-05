namespace Bhasha.Web.Domain;

public record Submission(
	Guid ProfileId,
	Guid ExpressionId,
	Translation Translation);

