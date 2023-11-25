namespace Bhasha.Shared.Domain;

public record Translation(
    int Id,
	string Language,
	string Text,
	string? Spoken,
	string? AudioId,
    Expression Expression)
{
	public static Translation Create(Expression expression, Language language, string text)
	{
		return new Translation(default, language, text, default, default, expression);
	}
}

