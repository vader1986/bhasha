using System;
using System.Linq;
using System.Text;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Exceptions;
using Bhasha.Common.MongoDB.Tests.Support;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Dto
{
    [TestFixture]
    public class ProcedureDtoTests
    {
        [Test]
        public void Convert_ProfileDto()
        {
            var dto = ProfileDtoBuilder.Build();
            var result = Converter.Convert(dto);

            Assert.That(result.Id, Is.EqualTo(dto.Id));
            Assert.That(result.From, Is.EqualTo(Language.Parse(dto.From)));
            Assert.That(result.To, Is.EqualTo(Language.Parse(dto.To)));
            Assert.That(result.Level, Is.EqualTo(dto.Level));
            Assert.That(result.UserId, Is.EqualTo(dto.UserId));
        }

        [Test]
        public void Convert_ProfileDto_with_invalid_language()
        {
            var dto = ProfileDtoBuilder.Build();
            dto.From = "asdf";

            Assert.Throws<InvalidDtoException>(() => Converter.Convert(dto));
        }

        [Test]
        public void Convert_Profile()
        {
            var profile = new Profile(Guid.NewGuid(), Guid.NewGuid(), Rnd.Create.Choose(Language.Supported.Values.ToArray()), Rnd.Create.Choose(Language.Supported.Values.ToArray()), Rnd.Create.Next(1, 10));
            var result = Converter.Convert(profile);

            Assert.That(result.Id, Is.EqualTo(profile.Id));
            Assert.That(result.UserId, Is.EqualTo(profile.UserId));
            Assert.That(result.From, Is.EqualTo(profile.From.ToString()));
            Assert.That(result.To, Is.EqualTo(profile.To.ToString()));
            Assert.That(result.Level, Is.EqualTo(profile.Level));
        }

        [Test]
        public void Convert_ChapterDto()
        {
            var dto = ChapterDtoBuilder.Build();
            var tokens = dto
                .Pages
                .Select(p => p.TokenId)
                .ToDictionary(
                    x => x,
                    x => TokenDtoBuilder.Build(x));

            var result = Converter.Convert(dto, tokens);

            Assert.That(result.Id, Is.EqualTo(dto.Id));
            Assert.That(result.Level, Is.EqualTo(dto.Level));
            Assert.That(result.Name, Is.EqualTo(dto.Name));
            Assert.That(result.Description, Is.EqualTo(dto.Description));
            Assert.That(result.PictureId.Id, Is.EqualTo(dto.PictureId));
            Assert.That(result.Pages.Length, Is.EqualTo(dto.Pages.Length));
        }

        [Test]
        public void Convert_ChapterDto_with_invalid_properties()
        {
            var dto = ChapterDtoBuilder.Build();
            var tokens = Enumerable.Repeat(TokenDtoBuilder.Build(), 1).ToDictionary(x => x.Id, x => x);

            Assert.Throws<InvalidDtoException>(() => Converter.Convert(dto, tokens));
        }

        [Test]
        public void Convert_Chapter()
        {
            var pages = new[] {
                new Page(
                    new Token(Guid.NewGuid(), Rnd.Create.NextString(), Rnd.Create.Next(), CEFR.A1, TokenType.Adjective, Rnd.Create.NextStrings().ToArray(), ResourceId.Create(Rnd.Create.NextString())), PageType.ChooseSolution,
                    new LanguageToken(Language.Bengoli, Rnd.Create.NextString(), Rnd.Create.NextString(), ResourceId.Create(Rnd.Create.NextString())), Rnd.Create.NextStrings().ToArray())
            };
            var chapter = new Chapter(Guid.NewGuid(), Rnd.Create.Next(), Rnd.Create.NextString(), Rnd.Create.NextPhrase(), pages, ResourceId.Create(Rnd.Create.NextString()));

            var result = Converter.Convert(chapter);

            Assert.That(result.Id, Is.EqualTo(chapter.Id));
            Assert.That(result.Level, Is.EqualTo(chapter.Level));
            Assert.That(result.Name, Is.EqualTo(chapter.Name));
            Assert.That(result.Description, Is.EqualTo(chapter.Description));
            Assert.That(result.PictureId, Is.EqualTo(chapter.PictureId.Id));
            Assert.That(result.Pages.Length, Is.EqualTo(chapter.Pages.Length));
            Assert.That(result.Pages[0].Language, Is.EqualTo(chapter.Pages[0].Word.Language.ToString()));
            Assert.That(result.Pages[0].PageType, Is.EqualTo(chapter.Pages[0].PageType.ToString()));
            Assert.That(result.Pages[0].TokenId, Is.EqualTo(chapter.Pages[0].Token.Id));
            Assert.That(result.Pages[0].Arguments, Is.EquivalentTo(chapter.Pages[0].Arguments));
        }

        [Test]
        public void Convert_ChapterStatsDto()
        {
            var dto = ChapterStatsDtoBuilder.Build();
            var result = Converter.Convert(dto);

            Assert.That(result.ProfileId, Is.EqualTo(dto.ProfileId));
            Assert.That(result.ChapterId, Is.EqualTo(dto.ChapterId));
            Assert.That(result.Completed, Is.EqualTo(dto.Completed));
            Assert.That(result.Tips, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Tips)));
            Assert.That(result.Submits, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Submits)));
            Assert.That(result.Failures, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Failures)));
        }

        [Test]
        public void Convert_ChapterStatsDto_with_invalid_properties()
        {
            var dto = new ChapterStatsDto();
            
            Assert.Throws<InvalidDtoException>(() => Converter.Convert(dto));
        }

        [Test]
        public void Convert_ChapterStats()
        {
            var tips = Rnd.Create.NextString(5);
            var submits = Rnd.Create.NextString(5);
            var failures = Rnd.Create.NextString(5);
            var stats = new ChapterStats(Guid.NewGuid(), Guid.NewGuid(), Rnd.Create.Next(1) == 0, Encoding.UTF8.GetBytes(tips), Encoding.UTF8.GetBytes(submits), Encoding.UTF8.GetBytes(failures));

            var result = Converter.Convert(stats);

            Assert.That(result.ChapterId, Is.EqualTo(stats.ChapterId));
            Assert.That(result.ProfileId, Is.EqualTo(stats.ProfileId));
            Assert.That(result.Completed, Is.EqualTo(stats.Completed));
            Assert.That(result.Tips, Is.EqualTo(tips));
            Assert.That(result.Submits, Is.EqualTo(submits));
            Assert.That(result.Failures, Is.EqualTo(failures));
        }

        [Test]
        public void Convert_UserDto()
        {
            var dto = new UserDto { Id = Guid.NewGuid(), Email = Rnd.Create.NextString(), UserName = Rnd.Create.NextString() };
            var result = Converter.Convert(dto);

            Assert.That(result.Id, Is.EqualTo(dto.Id));
            Assert.That(result.UserName, Is.EqualTo(dto.UserName));
            Assert.That(result.EmailAddress, Is.EqualTo(dto.Email));
        }

        [Test]
        public void Convert_User()
        {
            var user = new User(Guid.NewGuid(), Rnd.Create.NextString(), Rnd.Create.NextString());
            var result = Converter.Convert(user);

            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.UserName, Is.EqualTo(user.UserName));
            Assert.That(result.Email, Is.EqualTo(user.EmailAddress));
        }

        [Test]
        public void Convert_TipDto()
        {
            var dto = new TipDto { Id = Guid.NewGuid(), ChapterId = Guid.NewGuid(), PageIndex = Rnd.Create.Next(), Text = Rnd.Create.NextString() };
            var result = Converter.Convert(dto);

            Assert.That(result.Id, Is.EqualTo(dto.Id));
            Assert.That(result.ChapterId, Is.EqualTo(dto.ChapterId));
            Assert.That(result.PageIndex, Is.EqualTo(dto.PageIndex));
            Assert.That(result.Text, Is.EqualTo(dto.Text));
        }

        [Test]
        public void Convert_Tip()
        {
            var tip = new Tip(Guid.NewGuid(), Guid.NewGuid(), Rnd.Create.Next(), Rnd.Create.NextString());
            var result = Converter.Convert(tip);

            Assert.That(result.Id, Is.EqualTo(tip.Id));
            Assert.That(result.ChapterId, Is.EqualTo(tip.ChapterId));
            Assert.That(result.PageIndex, Is.EqualTo(tip.PageIndex));
            Assert.That(result.Text, Is.EqualTo(tip.Text));
        }
    }
}