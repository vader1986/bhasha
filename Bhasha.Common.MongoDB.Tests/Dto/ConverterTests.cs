using System.Linq;
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
        public void Convert_ProcedureDto_with_invalid_values()
        {
            var dto = new ProcedureDto {
                Support = null
            };

            Assert.Throws<InvalidProcedureException>(() => Converter.Convert(dto));
        }

        [Test]
        public void Convert_ProcedureDto_without_audio_id()
        {
            var dto = new ProcedureDto {
                ProcedureId = "ID-123",
                Description = "my description",
                Support = new string[0],
                Tutorial = new [] { "TutorialID-123" }
            };

            var procedure = Converter.Convert(dto);

            Assert.That(procedure.AudioId, Is.Null);
            Assert.That(procedure.Id, Is.EqualTo(new ProcedureId(dto.ProcedureId)));
            Assert.That(procedure.Description, Is.EqualTo(dto.Description));
            Assert.That(procedure.Support, Is.EquivalentTo(new string[0]));
            Assert.That(procedure.Tutorial, Is.EquivalentTo(new[] { ResourceId.Create(dto.Tutorial[0]) }));
        }

        [Test]
        public void Convert_ProcedureDto_without_tutorial()
        {
            var dto = new ProcedureDto
            {
                ProcedureId = "ID-123",
                Description = "my description",
                Support = new string[0],
                AudioId = "AudioID-123"
            };

            var procedure = Converter.Convert(dto);

            Assert.That(procedure.Tutorial, Is.EquivalentTo(new ResourceId[0]));
            Assert.That(procedure.AudioId, Is.EqualTo(ResourceId.Create(dto.AudioId)));
            Assert.That(procedure.Id, Is.EqualTo(new ProcedureId(dto.ProcedureId)));
            Assert.That(procedure.Description, Is.EqualTo(dto.Description));
            Assert.That(procedure.Support, Is.EquivalentTo(new string[0]));
        }

        [Test]
        public void Convert_ProcedureDto_with_custom_token_type([Values]TokenType supportedToken)
        {
            var dto = new ProcedureDto
            {
                ProcedureId = "ID-123",
                Description = "my description",
                Support = new[] { supportedToken.ToString() },
                AudioId = "AudioID-123",
                Tutorial = new[] { "TutorialID-123" }
            };

            var procedure = Converter.Convert(dto);

            Assert.That(procedure.AudioId, Is.EqualTo(ResourceId.Create(dto.AudioId)));
            Assert.That(procedure.Id, Is.EqualTo(new ProcedureId(dto.ProcedureId)));
            Assert.That(procedure.Description, Is.EqualTo(dto.Description));
            Assert.That(procedure.Support, Is.EquivalentTo(new[] { supportedToken }));
            Assert.That(procedure.Tutorial, Is.EquivalentTo(new[] { ResourceId.Create(dto.Tutorial[0]) }));
        }

        [Test]
        public void Convert_TranslationDto([Values] LanguageLevel level, [Values] TokenType tokenType)
        {
            var from = TokenDtoBuilder
                .Default
                .WithLanguageId(Languages.English)
                .Build();

            var to = TokenDtoBuilder
                .Default
                .WithLanguageId(Languages.Bengoli)
                .Build();

            var dto = TranslationDtoBuilder
                .Default
                .WithTokens(from, to)
                .WithLevel(level.ToString())
                .WithTokenType(tokenType.ToString())
                .Build();

            var translation = Converter.Convert(dto, Languages.English.ToString(), Languages.Bengoli.ToString());

            Assert.That(translation.Reference, Is.Not.Null);
            Assert.That(translation.Reference.Label, Is.EqualTo(dto.Label));
            Assert.That(translation.Reference.Id.GroupId, Is.EqualTo(dto.GroupId));
            Assert.That(translation.Reference.Id.SequenceNumber, Is.EqualTo(dto.SequenceNumber));
            Assert.That(translation.Reference.Level, Is.EqualTo(level));
            Assert.That(translation.Reference.Categories, Is.EquivalentTo(dto.Categories.Select(x => new Category(x))));
            Assert.That(translation.Reference.PictureId, Is.EqualTo(ResourceId.Create(dto.PictureId)));
            Assert.That(translation.Reference.TokenType, Is.EqualTo(tokenType));

            Assert.That(translation.From, Is.Not.Null);
            Assert.That(translation.From.Language, Is.EqualTo(Language.Parse(from.LanguageId)));
            Assert.That(translation.From.Native, Is.EqualTo(from.Native));
            Assert.That(translation.From.Spoken, Is.EqualTo(from.Spoken));
            Assert.That(translation.From.AudioId, Is.EqualTo(ResourceId.Create(from.AudioId)));

            Assert.That(translation.To, Is.Not.Null);
            Assert.That(translation.To.Language, Is.EqualTo(Language.Parse(to.LanguageId)));
            Assert.That(translation.To.Native, Is.EqualTo(to.Native));
            Assert.That(translation.To.Spoken, Is.EqualTo(to.Spoken));
            Assert.That(translation.To.AudioId, Is.EqualTo(ResourceId.Create(to.AudioId)));
        }

        [Test]
        public void Convert_TranslationDto_invalid_dto()
        {
            var dto = new TranslationDto();

            Assert.Throws<InvalidTranslationException>(()
                => Converter.Convert(dto, Languages.English.ToString(), Languages.Bengoli.ToString()));
        }

        [Test]
        public void Convert_UserProgressDto()
        {
            var dto = UserProgressDtoBuilder.Create();
            var result = Converter.Convert(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(new EntityId(dto.UserId)));
            Assert.That(result.From, Is.EqualTo(Language.Parse(dto.From)));
            Assert.That(result.To, Is.EqualTo(Language.Parse(dto.To)));
            Assert.That(result.Stats.Level, Is.EqualTo(LanguageLevel.B2));
            Assert.That(result.Stats.GroupId, Is.EqualTo(dto.GroupId));
            Assert.That(result.Stats.CompletedChapters, Is.EqualTo(dto.CompletedChapters));
            Assert.That(result.Stats.CompletedTokens, Is.EqualTo(dto.CompletedTokens));
            Assert.That(result.Stats.CompletedSequenceNumbers, Is.EquivalentTo(dto.CompletedSequenceNumbers));
        }

        [Test]
        public void Convert_UserProgressDto_invalid()
        {
            var dto = new UserProgressDto();

            Assert.Throws<InvalidUserProgressException>(() => Converter.Convert(dto));
        }
    }
}