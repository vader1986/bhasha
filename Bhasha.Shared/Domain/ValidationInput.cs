namespace Bhasha.Shared.Domain;

public record ValidationInput(
	ProfileKey Key,
	int ExpressionId,
	Translation Translation);

