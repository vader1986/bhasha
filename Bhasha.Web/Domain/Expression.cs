namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record Expression(
    Guid Id,
	ExpressionType? ExpressionType,
	PartOfSpeech? PartOfSpeech,
	CEFR? Cefr,
	string? ResourceId,
	string[] Labels,
    string[] Synonyms,
    int Level)
{
	public static Expression Create(int level)
	{
		return new Expression(Guid.Empty, default, default, default, default, Array.Empty<string>(), Array.Empty<string>(), level);
	}
}
