using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bhasha.Common.MongoDB.Exceptions;

namespace Bhasha.Common.MongoDB.Dto
{
    public static class Converter
    {
        public static Profile Convert(ProfileDto dto)
        {
            try
            {
                return new Profile(
                    dto.Id, dto.UserId,
                    Language.Parse(dto.From),
                    Language.Parse(dto.To),
                    dto.Level);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public static ProfileDto Convert(Profile profile)
        {
            return new ProfileDto {
                Id = profile.Id,
                UserId = profile.UserId,
                From = profile.From,
                To = profile.To,
                Level = profile.Level
            };
        }

        public static Token Convert(TokenDto dto)
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
                    ResourceId.Create(dto.PictureId));
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public static Chapter Convert(ChapterDto dto, IDictionary<Guid, TokenDto> tokens)
        {
            try
            {
                Page ConvertPage(PageDto page)
                {
                    var token = tokens[page.TokenId];
                    var translation = token.Translations[page.Language];
                    var audioId = ResourceId.Create(translation.AudioId);

                    return new Page(
                        Convert(token),
                        Enum.Parse<PageType>(page.PageType),
                        new LanguageToken(
                            Language.Parse(page.Language),
                            translation.Native,
                            translation.Spoken,
                            audioId),
                        page.Arguments);
                }

                var pages = dto
                    .Pages
                    .Select(ConvertPage)
                    .ToArray();

                return new Chapter(
                    dto.Id,
                    dto.Level,
                    dto.Name,
                    dto.Description,
                    pages,
                    ResourceId.Create(dto.PictureId));
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto, tokens);
            }
        }

        public static ChapterDto Convert(Chapter chapter)
        {
            return new ChapterDto {
                Id = chapter.Id,
                Level = chapter.Level,
                Name = chapter.Name,
                Description = chapter.Description,
                Pages = chapter.Pages.Select(x => new PageDto {
                    TokenId = x.Token.Id,
                    PageType = x.PageType.ToString(),
                    Language = x.Word.Language,
                    Arguments = x.Arguments
                }).ToArray(),
                PictureId = chapter.PictureId?.Id
            };
        }

        public static byte[] Convert(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string Convert(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        public static ChapterStats Convert(ChapterStatsDto dto)
        {
            try
            {
                return new ChapterStats(
                    dto.ProfileId,
                    dto.ChapterId,
                    dto.Completed,
                    Convert(dto.Tips),
                    Convert(dto.Submits),
                    Convert(dto.Failures));
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public static ChapterStatsDto Convert(ChapterStats stats)
        {
            return new ChapterStatsDto
            {
                ProfileId = stats.ProfileId,
                ChapterId = stats.ChapterId,
                Completed = stats.Completed,
                Tips = Convert(stats.Tips),
                Submits = Convert(stats.Submits),
                Failures = Convert(stats.Failures)
            };
        }

        public static User Convert(UserDto dto)
        {
            try
            {
                return new User(dto.Id, dto.UserName, dto.Email);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public static UserDto Convert(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.EmailAddress
            };
        }

        public static Tip Convert(TipDto dto)
        {
            try
            {
                return new Tip(dto.Id, dto.ChapterId, dto.PageIndex, dto.Text);
            }
            catch (Exception e)
            {
                throw new InvalidDtoException(e, dto);
            }
        }

        public static TipDto Convert(Tip tip)
        {
            return new TipDto
            {
                Id = tip.Id,
                ChapterId = tip.ChapterId,
                PageIndex = tip.PageIndex,
                Text = tip.Text
            };
        }
    }
}
