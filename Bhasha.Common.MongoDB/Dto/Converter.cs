using System;
using System.Linq;
using System.Text;
using Bhasha.Common.MongoDB.Exceptions;

namespace Bhasha.Common.MongoDB.Dto
{
    public class Converter :
        IConvert<GenericPageDto, GenericPage>,
        IConvert<GenericChapterDto, GenericChapter>,
        IConvert<ChapterStatsDto, ChapterStats>,
        IConvert<ProfileDto, Profile>,
        IConvert<TokenDto, Token>,
        IConvert<TranslationDto, Translation>
    {
        public GenericPage Convert(GenericPageDto dto)
        {
            try
            {
                return new GenericPage(
                    dto.TokenId,
                    Enum.Parse<PageType>(dto.PageType),
                    dto.TipIds);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public GenericPageDto Convert(GenericPage product)
        {
            return new GenericPageDto
            {
                TokenId = product.TokenId,
                PageType = product.PageType.ToString(),
                TipIds = product.TipIds
            };
        }

        public GenericChapter Convert(GenericChapterDto dto)
        {
            try
            {
                return new GenericChapter(
                    dto.Id,
                    dto.Level,
                    dto.NameId,
                    dto.DescriptionId,
                    dto.Pages.Select(Convert).ToArray());
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public GenericChapterDto Convert(GenericChapter product)
        {
            return new GenericChapterDto
            {
                Id = product.Id,
                Level = product.Level,
                NameId = product.NameId,
                DescriptionId = product.DescriptionId,
                Pages = product.Pages.Select(Convert).ToArray()
            };
        }

        public ChapterStats Convert(ChapterStatsDto dto)
        {
            try
            {
                return new ChapterStats(
                    dto.Id,
                    dto.ProfileId,
                    dto.ChapterId,
                    dto.Completed,
                    dto.Tips,
                    Encoding.UTF8.GetBytes(dto.Submits),
                    Encoding.UTF8.GetBytes(dto.Failures));
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public ChapterStatsDto Convert(ChapterStats product)
        {
            return new ChapterStatsDto
            {
                Id = product.Id,
                ChapterId = product.ChapterId,
                ProfileId = product.ProfileId,
                Completed = product.Completed,
                Tips = product.Tips,
                Submits = Encoding.UTF8.GetString(product.Submits),
                Failures = Encoding.UTF8.GetString(product.Failures)
            };
        }

        public Profile Convert(ProfileDto dto)
        {
            try
            {
                return new Profile(
                    dto.Id,
                    dto.UserId,
                    dto.From,
                    dto.To,
                    dto.Level,
                    dto.CompletedChapters);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public ProfileDto Convert(Profile product)
        {
            return new ProfileDto
            {
                Id = product.Id,
                UserId = product.UserId,
                From = product.From,
                To = product.To,
                Level = product.Level,
                CompletedChapters = product.CompletedChapters
            };
        }

        public Token Convert(TokenDto dto)
        {
            try
            {
                return new Token(
                    dto.Id,
                    dto.Label,
                    dto.Level,
                    Enum.Parse<CEFR>(dto.Cefr),
                    Enum.Parse<TokenType>(dto.TokenType),
                    dto.Categories,
                    dto.PictureId);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public TokenDto Convert(Token product)
        {
            return new TokenDto
            {
                Id = product.Id,
                Label = product.Label,
                Level = product.Level,
                Cefr = product.Cefr.ToString(),
                TokenType = product.TokenType.ToString(),
                Categories = product.Categories,
                PictureId = product.PictureId
            };
        }

        public Translation Convert(TranslationDto dto)
        {
            try
            {
                return new Translation(
                    dto.Id,
                    dto.TokenId,
                    dto.Language,
                    dto.Native,
                    dto.Spoken,
                    dto.AudioId);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public TranslationDto Convert(Translation product)
        {
            return new TranslationDto
            {
                Id = product.Id,
                TokenId = product.TokenId,
                Language = product.Language,
                Native = product.Native,
                Spoken = product.Spoken,
                AudioId = product.AudioId
            };
        }
    }
}
