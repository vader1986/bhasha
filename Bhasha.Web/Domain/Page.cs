namespace Bhasha.Web.Domain;

[GenerateSerializer]
public record Page(PageType PageType, Guid ExpressionId);