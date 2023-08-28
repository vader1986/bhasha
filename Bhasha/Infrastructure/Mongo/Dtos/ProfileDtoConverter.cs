using Bhasha.Domain;

namespace Bhasha.Infrastructure.Mongo.Dtos;

public static class ProfileDtoConverter
{
    public static Profile Convert(this ProfileDto dto)
    {
        return new Profile(
            dto.Id,
            new Domain.ProfileKey(
                dto.Key.UserId,
                new LangKey(
                    dto.Key.Native,
                    dto.Key.Target)),
            dto.Level,
            dto.CompletedChapters,
            dto.CurrentChapter.Convert());
    }

    private static ChapterSelection? Convert(this ChapterSelectionDto? dto)
    {
        if (dto is null) return null;
        return new ChapterSelection(
            dto.ChapterId,
            dto.PageIndex,
            dto.Pages
                .Select(page => 
                    page switch
                    {
                        ValidationResult.Correct => Domain.ValidationResult.Correct,
                        ValidationResult.Wrong => Domain.ValidationResult.Wrong,
                        ValidationResult.PartiallyCorrect => Domain.ValidationResult.PartiallyCorrect,
                        _ => throw new ArgumentOutOfRangeException(nameof(dto))
                    })
                .ToArray());
    }

    public static ProfileDto Convert(this Profile dto)
    {
        return new ProfileDto(
            dto.Id,
            new ProfileKey(
                dto.Key.UserId,
                dto.Key.LangId.Native,
                dto.Key.LangId.Target),
            dto.Level,
            dto.CompletedChapters,
            dto.CurrentChapter.Convert());
    }
    
    private static ChapterSelectionDto? Convert(this ChapterSelection? dto)
    {
        if (dto is null) return null;
        return new ChapterSelectionDto(
            dto.ChapterId,
            dto.PageIndex,
            dto.Pages
                .Select(page => 
                    page switch
                    {
                        Domain.ValidationResult.Correct => ValidationResult.Correct,
                        Domain.ValidationResult.Wrong => ValidationResult.Wrong,
                        Domain.ValidationResult.PartiallyCorrect => ValidationResult.PartiallyCorrect,
                        _ => throw new ArgumentOutOfRangeException(nameof(dto))
                    })
                .ToArray());
    }
}