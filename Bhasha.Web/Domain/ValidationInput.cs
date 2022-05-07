namespace Bhasha.Web.Domain
{
	public record ValidationInput(
		Profile Profile,
		Guid ExpressionId,
		Translation Translation);
}

