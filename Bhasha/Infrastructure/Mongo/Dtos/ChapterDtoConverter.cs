using Bhasha.Domain;

namespace Bhasha.Infrastructure.Mongo.Dtos;

public static class ChapterDtoConverter
{
    public static Chapter Convert(this ChapterDto dto)
    {
        return new Chapter(
            dto.Id,
            dto.RequiredLevel,
            dto.NameId,
            dto.DescriptionId,
            dto.Pages
                .Select(page => new Page(
                    page.PageType switch
                    {
                        PageType.ClozeChoice => Domain.PageType.ClozeChoice,
                        PageType.ClozeFillout => Domain.PageType.ClozeFillout,
                        PageType.MultipleChoice => Domain.PageType.MultipleChoice,
                        _ => throw new ArgumentOutOfRangeException(nameof(dto))
                    },
                    page.ExpressionId))
                .ToArray(),
            dto.ResourceId,
            dto.AuthorId);
    }

    public static ChapterDto Convert(this Chapter dto)
    {
        return new ChapterDto(
            dto.Id,
            dto.RequiredLevel,
            dto.NameId,
            dto.DescriptionId,
            dto.Pages
                .Select(page => new PageDto(
                    page.PageType switch
                    {
                        Domain.PageType.ClozeChoice => PageType.ClozeChoice,
                        Domain.PageType.ClozeFillout => PageType.ClozeFillout,
                        Domain.PageType.MultipleChoice => PageType.MultipleChoice,
                        _ => throw new ArgumentOutOfRangeException(nameof(dto))
                    },
                    page.ExpressionId))
                .ToArray(),
            dto.ResourceId,
            dto.AuthorId);
    }
}