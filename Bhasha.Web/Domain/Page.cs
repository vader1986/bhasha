namespace Bhasha.Web.Domain;

public record Page(
	PageType PageType,
	Guid ExpressionId,
	string[] Leads);