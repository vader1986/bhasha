namespace Bhasha.Web.Domain
{
	public record Profile(
		Guid Id,
		string UserId,
		string Native,
		string Target,
		Progress Progress);
}