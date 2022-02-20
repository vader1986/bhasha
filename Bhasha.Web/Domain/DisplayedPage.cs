namespace Bhasha.Web.Domain
{
	public record DisplayedPage(
		string Expression,
		string? Spoken,
		string? Lead,
		bool HasLead);

	public record DisplayedPage<T>(
		string Expression,
		string? Spoken,
		string? Lead,
		bool HasLead,
		T Arguments) : DisplayedPage(Expression, Spoken, Lead, HasLead);
}
