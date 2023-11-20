using Bhasha.Shared.Domain;

namespace Bhasha.MongoDb.Infrastructure.Mongo.Dtos;

public static class TranslationDtoConverter
{
    public static Translation Convert(this TranslationDto dto)
    {
        return new Translation(
            dto.Id,
            dto.ExpressionId,
            dto.Language,
            dto.Text,
            dto.Spoken,
            dto.AudioId);
    }

    public static TranslationDto Convert(this Translation dto)
    {
        return new TranslationDto(
            dto.Id,
            dto.ExpressionId,
            dto.Language,
            dto.Text,
            dto.Spoken,
            dto.AudioId);
    }
}