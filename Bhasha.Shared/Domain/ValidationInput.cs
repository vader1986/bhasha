namespace Bhasha.Shared.Domain;

public record ValidationInput(
	ProfileKey Key,
    Guid ExpressionId,
	Translation Translation);

