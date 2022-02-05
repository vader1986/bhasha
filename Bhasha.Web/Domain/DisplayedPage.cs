namespace Bhasha.Web.Domain
{
	public record DisplayedPage(
		string Expression,
		string? Spoken,
		string? Lead);

	public record DisplayedPage<T>(
		string Expression,
		string? Spoken,
		string? Lead,
		T Arguments) : DisplayedPage(Expression, Spoken, Lead);
}
