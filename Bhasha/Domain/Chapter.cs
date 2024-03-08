using Generator.Equals;

namespace Bhasha.Domain;

[Equatable]
public partial record Chapter(
	int Id,
	int RequiredLevel,
	Expression Name,
	Expression Description,
	[property: OrderedEquality]
	Expression[] Pages,
	string? ResourceId,
	string AuthorId);