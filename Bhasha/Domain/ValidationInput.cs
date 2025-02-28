namespace Bhasha.Domain;

public record ValidationInput(
	ProfileKey Key,
	int ExpressionId,
	string UserInput);

