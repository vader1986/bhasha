using Generator.Equals;

namespace Bhasha.MongoDb.Infrastructure.Mongo.Dtos;

[Equatable]
public partial record ChapterDto(
	Guid Id,
	
	int RequiredLevel,
    
	Guid NameId,
    Guid DescriptionId,
	
	[property: OrderedEquality]
	PageDto[] Pages,
	
	string? ResourceId,
	string AuthorId);
	

public record PageDto(
	PageType PageType, 
	Guid ExpressionId);
	
public enum PageType
{
	MultipleChoice,
	ClozeChoice,
	ClozeFillout
}

public record ClozeDto(
	string[] Words, 
	int[] Gaps);

public record ClozeChoiceDto(
	string[] Text,
	int[] Gaps,
	string[] Choices);

public record MultipleChoiceDto(
	TranslationDto[] Choices);