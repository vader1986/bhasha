namespace Bhasha.Shared.Domain;

public record Translation(
    Guid Id,
    Guid ExpressionId,
	string Language,
	string Text,
	string? Spoken,
	string? AudioId)
{
	public static Translation Create(Guid expressionId, Language language, string text)
	{
		return new Translation(Guid.Empty, expressionId, language, text, default, default);
	}
}

