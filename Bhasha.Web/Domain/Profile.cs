namespace Bhasha.Web.Domain
{
	public record Profile(
		Guid Id,
		string UserId,
		Language Native,
		Language Target,
		int Level,
		int CompletedChapters);
}