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
        IConvert<TipDto, Tip>,
        IConvert<TokenDto, Token>,
        IConvert<TranslationDto, Translation>,
        IConvert<UserDto, User>
    {
        public GenericPage Convert(GenericPageDto dto)
        {
            try
            {
                return new GenericPage(
                    dto.TokenId,
                    Enum.Parse<PageType>(dto.PageType));
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
                PageType = product.PageType.ToString()
            };
        }

        public GenericChapter Convert(GenericChapterDto dto)
        {
            try
            {
                return new GenericChapter(
                    dto.Id,
                    dto.Level,
                    dto.Name,
                    dto.Description,
                    dto.Pages.Select(Convert).ToArray(),
                    dto.PictureId);
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
                Name = product.Name,
                Description = product.Description,
                Pages = product.Pages.Select(Convert).ToArray(),
                PictureId = product.PictureId
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
                    Encoding.UTF8.GetBytes(dto.Tips),
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
                Tips = Encoding.UTF8.GetString(product.Tips),
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

        public Tip Convert(TipDto dto)
        {
            try
            {
                return new Tip(
                    dto.Id,
                    dto.ChapterId,
                    dto.PageIndex,
                    dto.Text);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public TipDto Convert(Tip product)
        {
            return new TipDto
            {
                Id = product.Id,
                ChapterId = product.ChapterId,
                PageIndex = product.PageIndex,
                Text = product.Text
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

        public User Convert(UserDto dto)
        {
            try
            {
                return new User(
                    dto.Id,
                    dto.UserName,
                    dto.Email);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public UserDto Convert(User product)
        {
            return new UserDto
            {
                Id = product.Id,
                UserName = product.UserName,
                Email = product.Email
            };
        }
    }
}
