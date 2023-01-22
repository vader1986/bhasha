namespace Bhasha.Domain;

[GenerateSerializer]
public record Page(PageType PageType, Guid ExpressionId);