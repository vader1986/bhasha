using Generator.Equals;

namespace Bhasha.Infrastructure.Mongo.Dtos;

[Equatable]
public partial record ProfileDto
(
    Guid Id,
    ProfileKey Key,
    int Level,
    
    [property: OrderedEquality]
    Guid[] CompletedChapters,
    
    ChapterSelectionDto? CurrentChapter
);

[Equatable]
public partial record ChapterSelectionDto
(
    Guid ChapterId,
    int PageIndex,

    [property:OrderedEquality]
    ValidationResult[] Pages
);

public record ProfileKey
(
    string UserId, 
    string Native, 
    string Target
);

public enum ValidationResult
{
    Correct,
    PartiallyCorrect,
    Wrong
}