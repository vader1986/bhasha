namespace Bhasha.Web.Domain
{
	public record Expression(
		Guid Id,
		ExpressionType? ExpressionType,
		PartOfSpeech? PartOfSpeech,
		CEFR? Cefr,
		string? ResourceId,
		string[] Labels,
		int Level);
}
