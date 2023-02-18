namespace Bhasha.Domain;

[GenerateSerializer]
public record ValidationInput(
	LangKey Languages,
    Guid ExpressionId,
	Translation Translation);

