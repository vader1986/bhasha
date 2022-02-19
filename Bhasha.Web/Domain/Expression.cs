namespace Bhasha.Web.Domain
{
	public record Expression(
		Guid Id,
		ExpressionType ExpressionType,
		CEFR Cefr,
		string? ResourceId,
		string[] Labels,
		int Level,
		Translation[] Translations);
}
