using Generator.Equals;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bhasha.Web.Domain;

[Equatable]
[GenerateSerializer]
public partial record Chapter(
	[property:BsonId(IdGenerator = typeof(GuidGenerator))]
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