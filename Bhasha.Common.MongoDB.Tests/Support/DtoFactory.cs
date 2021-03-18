using System;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class DtoFactory
    {
        public static TDto Build<TDto>() where TDto : MongoDB.Dto.Dto
        {
            if (typeof(TDto) == typeof(ChapterStatsDto))
            {
                return ChapterStatsDtoBuilder.Build() as TDto;
            }

            if (typeof(TDto) == typeof(GenericChapterDto))
            {
                return GenericChapterDtoBuilder.Build() as TDto;
            }

            if (typeof(TDto) == typeof(ProfileDto))
            {
                return ProfileDtoBuilder.Build() as TDto;
            }

            if (typeof(TDto) == typeof(TipDto))
            {
                return TipDtoBuilder.Build() as TDto;
            }

            if (typeof(TDto) == typeof(TokenDto))
            {
                return TokenDtoBuilder.Build() as TDto;
            }

            if (typeof(TDto) == typeof(TranslationDto))
            {
                return TranslationDtoBuilder.Build() as TDto;
            }

            if (typeof(TDto) == typeof(UserDto))
            {
                return UserDtoBuilder.Build() as TDto;
            }

            throw new InvalidOperationException(
                $"found no builder for {typeof(TDto).Name}");
        }
    }
}
