namespace Bhasha.Web.Domain
{
	public record Expression(
		Guid Id,
		ExpressionType ExpressionType,
		CEFR Cefr,
		string? ResourceId,
		Translation[] Translations);
}

