using Bhasha.Shared.Domain;

namespace Bhasha.MongoDb.Infrastructure.Mongo.Dtos;

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
                        PageType.ClozeChoice => Shared.Domain.PageType.ClozeChoice,
                        PageType.ClozeFillout => Shared.Domain.PageType.ClozeFillout,
                        PageType.MultipleChoice => Shared.Domain.PageType.MultipleChoice,
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
                        Shared.Domain.PageType.ClozeChoice => PageType.ClozeChoice,
                        Shared.Domain.PageType.ClozeFillout => PageType.ClozeFillout,
                        Shared.Domain.PageType.MultipleChoice => PageType.MultipleChoice,
                        _ => throw new ArgumentOutOfRangeException(nameof(dto))
                    },
                    page.ExpressionId))
                .ToArray(),
            dto.ResourceId,
            dto.AuthorId);
    }
}