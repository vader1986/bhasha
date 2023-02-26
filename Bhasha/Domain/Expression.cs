using Generator.Equals;

namespace Bhasha.Domain;

[Equatable]
[GenerateSerializer]
public partial record Expression(
    Guid Id,
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
	public static Expression Create(int level)
	{
		return new Expression(Guid.Empty, default, default, default, default, Array.Empty<string>(), Array.Empty<string>(), level);
	}
}
