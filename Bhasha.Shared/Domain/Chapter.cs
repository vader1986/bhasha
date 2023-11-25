using Generator.Equals;

namespace Bhasha.Shared.Domain;

[Equatable]
public partial record Chapter(
	int Id,
	int RequiredLevel,
	Expression Name,
	Expression Description,
	[property: OrderedEquality]
	Expression[] Pages,
	string? ResourceId,
	string AuthorId)
{
	public static Chapter Create(int requiredLevel, Expression name, Expression description, IEnumerable<Expression> pages, string authorId)
	{
		return new Chapter(default, requiredLevel, name, description, pages.ToArray(), default, authorId);
	}
}