using Generator.Equals;

namespace Bhasha.Domain;

[Equatable]
public partial record Expression(
	int Id,
	ExpressionType? ExpressionType,
	PartOfSpeech? PartOfSpeech,
	CEFR? Cefr,
	string? ResourceId,
    [property: OrderedEquality]
    string[] Labels,
    [property: OrderedEquality]
	string[] Synonyms,
    int Level)
{
	public static Expression Create(int level = 1)
	{
		return new Expression(default, default, default, default, default, Array.Empty<string>(), Array.Empty<string>(), level);
	}
}
