using Generator.Equals;

namespace Bhasha.Shared.Domain;

[Equatable]
public partial record Chapter(
	Guid Id,
	int RequiredLevel,
    Guid NameId,
    Guid DescriptionId,
	[property: OrderedEquality]
	Page[] Pages,
	string? ResourceId,
	string AuthorId)
{
	public static Chapter Create(int requiredLevel, Guid nameId, Guid descriptionId, IEnumerable<Page> pages, string authorId)
	{
		return new Chapter(Guid.Empty, requiredLevel, nameId, descriptionId, pages.ToArray(), default, authorId);
	}
}