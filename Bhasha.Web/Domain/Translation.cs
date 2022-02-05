namespace Bhasha.Web.Domain
{
	public record Translation(
		string Language,
		string Native,
		string? Spoken,
		string? AudioId);
}

