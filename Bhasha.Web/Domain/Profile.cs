namespace Bhasha.Web.Domain
{
	public record Profile(
		Guid Id,
		string UserId,
		string Native,
		string Target,
		int Level,
		int CompletedChapters);
}