using System;
using System.Text;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Exceptions;
using Bhasha.Common.MongoDB.Tests.Support;
using Bhasha.Common.Tests.Support;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Dto
{
    [TestFixture]
    public class ConverterTests
    {
        private Converter _converter;

        [SetUp]
        public void Before()
        {
            _converter = new Converter();
        }

        [Test]
        public void Convert_GenericChapterDto()
        {
            var tokenId = new[] { Guid.NewGuid(), Guid.NewGuid() };
            var dto = GenericChapterDtoBuilder.Build(tokenId);
            var result = _converter.Convert(dto);

            Assert.That(result.Id == dto.Id);
            Assert.That(result.Level == dto.Level);
            Assert.That(result.NameId == dto.NameId);
            Assert.That(result.DescriptionId == dto.DescriptionId);
            Assert.That(result.Pages.Length == dto.Pages.Length);

            for (int i = 0; i < result.Pages.Length; i++)
            {
                Assert.That(result.Pages[i].TokenId == dto.Pages[i].TokenId);
                Assert.That(result.Pages[i].PageType == Enum.Parse<PageType>(dto.Pages[i].PageType));
            }
        }

        [Test]
        public void Convert_GenericChapterDto_throws()
        {
            var dto = GenericChapterDtoBuilder.Build();
            dto.Pages = null;

            Assert.Throws<InvalidDtoException>(() => _converter.Convert(dto));
        }

        [Test]
        public void Convert_GenericChapter()
        {
            var genericChapter = GenericChapterBuilder.Default.Build();
            var result = _converter.Convert(genericChapter);

            Assert.That(result.Id == genericChapter.Id);
            Assert.That(result.Level == genericChapter.Level);
            Assert.That(result.NameId == genericChapter.NameId);
            Assert.That(result.DescriptionId == genericChapter.DescriptionId);
            Assert.That(result.Pages.Length == genericChapter.Pages.Length);

            for (int i = 0; i < result.Pages.Length; i++)
            {
                Assert.That(result.Pages[i].TokenId == genericChapter.Pages[i].TokenId);
                Assert.That(result.Pages[i].PageType == genericChapter.Pages[i].PageType.ToString());
            }
        }

        [Test]
        public void Convert_ChapterStatsDto()
        {
            var dto = ChapterStatsDtoBuilder.Build();
            var result = _converter.Convert(dto);

            Assert.That(result.Id == dto.Id);
            Assert.That(result.ChapterId == dto.ChapterId);
            Assert.That(result.ProfileId == dto.ProfileId);
            Assert.That(result.Completed == dto.Completed);
            Assert.That(result.Tips, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Tips)));
            Assert.That(result.Submits, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Submits)));
            Assert.That(result.Failures, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Failures)));
        }

        [Test]
        public void Convert_ChapterStatsDto_throws()
        {
            var dto = ChapterStatsDtoBuilder.Build();
            dto.Tips = null;

            Assert.Throws<InvalidDtoException>(() => _converter.Convert(dto));
        }

        [Test]
        public void Convert_ChapterStats()
        {
            var stats = ChapterStatsBuilder.Default.Build();
            var result = _converter.Convert(stats);

            Assert.That(result.Id == stats.Id);
            Assert.That(result.ChapterId == stats.ChapterId);
            Assert.That(result.ProfileId == stats.ProfileId);
            Assert.That(result.Completed == stats.Completed);
            Assert.That(result.Tips == Encoding.UTF8.GetString(stats.Tips));
            Assert.That(result.Submits == Encoding.UTF8.GetString(stats.Submits));
            Assert.That(result.Failures == Encoding.UTF8.GetString(stats.Failures));
        }

        [Test]
        public void Convert_ProfileDto()
        {
            var dto = ProfileDtoBuilder.Build();
            var result = _converter.Convert(dto);

            Assert.That(result.Id == dto.Id);
            Assert.That(result.UserId == dto.UserId);
            Assert.That(result.From == dto.From);
            Assert.That(result.To == dto.To);
            Assert.That(result.Level == dto.Level);
        }

        [Test]
        public void Convert_ProfileDto_throws()
        {
            var dto = ProfileDtoBuilder.Build();
            dto.From = null;

            Assert.Throws<InvalidDtoException>(() => _converter.Convert(dto));
        }

        [Test]
        public void Convert_Profile()
        {
            var profile = ProfileBuilder.Default.Build();
            var result = _converter.Convert(profile);

            Assert.That(result.Id == profile.Id);
            Assert.That(result.UserId == profile.UserId);
            Assert.That(result.From == profile.From);
            Assert.That(result.To == profile.To);
            Assert.That(result.Level == profile.Level);
        }

        [Test]
        public void Convert_TipDto()
        {
            var dto = TipDtoBuilder.Build();
            var result = _converter.Convert(dto);

            Assert.That(result.Id == dto.Id);
            Assert.That(result.ChapterId == dto.ChapterId);
            Assert.That(result.PageIndex == dto.PageIndex);
            Assert.That(result.Text == dto.Text);
        }

        [Test]
        public void Convert_TipDto_throws()
        {
            var dto = TipDtoBuilder.Build();
            dto.PageIndex = -1;

            Assert.Throws<InvalidDtoException>(() => _converter.Convert(dto));
        }

        [Test]
        public void Convert_Tip()
        {
            var tip = TipBuilder.Default.Build();
            var result = _converter.Convert(tip);

            Assert.That(result.Id == tip.Id);
            Assert.That(result.ChapterId == tip.ChapterId);
            Assert.That(result.PageIndex == tip.PageIndex);
            Assert.That(result.Text == tip.Text);
        }

        [Test]
        public void Convert_TokenDto()
        {
            var dto = TokenDtoBuilder.Build();
            var result = _converter.Convert(dto);

            Assert.That(result.Id == dto.Id);
            Assert.That(result.Label == dto.Label);
            Assert.That(result.Level == dto.Level);
            Assert.That(result.Cefr.ToString() == dto.Cefr);
            Assert.That(result.TokenType.ToString() == dto.TokenType);
            Assert.That(result.PictureId == dto.PictureId);
        }

        [Test]
        public void Convert_TokenDto_throws()
        {
            var dto = TokenDtoBuilder.Build();
            dto.TokenType = "bla bla";

            Assert.Throws<InvalidDtoException>(() => _converter.Convert(dto));
        }

        [Test]
        public void Convert_Token()
        {
            var token = TokenBuilder.Default.Build();
            var result = _converter.Convert(token);

            Assert.That(result.Id == token.Id);
            Assert.That(result.Label == token.Label);
            Assert.That(result.Level == token.Level);
            Assert.That(result.Cefr == token.Cefr.ToString());
            Assert.That(result.TokenType == token.TokenType.ToString());
            Assert.That(result.PictureId == token.PictureId);
        }

        [Test]
        public void Convert_TranslationDto()
        {
            var dto = TranslationDtoBuilder.Build();
            var result = _converter.Convert(dto);

            Assert.That(result.TokenId == dto.TokenId);
            Assert.That(result.Language == dto.Language);
            Assert.That(result.Native == dto.Native);
            Assert.That(result.Spoken == dto.Spoken);
            Assert.That(result.AudioId == dto.AudioId);
        }

        [Test]
        public void Convert_TranslationDto_throws()
        {
            var dto = TranslationDtoBuilder.Build();
            dto.Language = "bla bla";

            Assert.Throws<InvalidDtoException>(() => _converter.Convert(dto));
        }

        [Test]
        public void Convert_Translation()
        {
            var translation = TranslationBuilder.Default.Build();
            var result = _converter.Convert(translation);

            Assert.That(result.TokenId == translation.TokenId);
            Assert.That(result.Language == translation.Language);
            Assert.That(result.Native == translation.Native);
            Assert.That(result.Spoken == translation.Spoken);
            Assert.That(result.AudioId == translation.AudioId);
        }
    }
}