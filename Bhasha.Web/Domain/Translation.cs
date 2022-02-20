namespace Bhasha.Web.Domain
{
	public record Translation(
		Guid Id,
		Guid ExpressionId,
		string Language,
		string Native,
		string? Spoken,
		string? AudioId);
}

