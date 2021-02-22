using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Exceptions;
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
        public void Convert_ProcedureDto_without_tutorial()
        {
            var dto = new ProcedureDto {
                ProcedureId = "ID-123",
                Description = "my description",
                Support = new string[0],
                Tutorial = new [] { "TutorialID-123" }
            };

            var procedure = Converter.Convert(dto);

            Assert.That(procedure.Audio, Is.Null);
            Assert.That(procedure.Id, Is.EqualTo(new ProcedureId(dto.ProcedureId)));
            Assert.That(procedure.Description, Is.EqualTo(dto.Description));
            Assert.That(procedure.Support, Is.EquivalentTo(new string[0]));
            Assert.That(procedure.Tutorial, Is.EquivalentTo(new[] { ResourceId.Create(dto.Tutorial[0]) }));
        }

        [Test]
        public void Convert_ProcedureDto_without_audio()
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
            Assert.That(procedure.Audio, Is.EqualTo(ResourceId.Create(dto.AudioId)));
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

            Assert.That(procedure.Audio, Is.EqualTo(ResourceId.Create(dto.AudioId)));
            Assert.That(procedure.Id, Is.EqualTo(new ProcedureId(dto.ProcedureId)));
            Assert.That(procedure.Description, Is.EqualTo(dto.Description));
            Assert.That(procedure.Support, Is.EquivalentTo(new[] { supportedToken }));
            Assert.That(procedure.Tutorial, Is.EquivalentTo(new[] { ResourceId.Create(dto.Tutorial[0]) }));
        }

        [Test]
        public void Convert_TranslationDto_fully_populated_dto([Values] LanguageLevel level, [Values] TokenType tokenType)
        {
            var dto = new TranslationDto
            {
                Label = "cat",
                Level = level.ToString(),
                Categories = new[] { "pet", "animal" },
                PictureId = "PicID-123",
                TokenType = tokenType.ToString(),
                Tokens = new[] {
                    new TokenDto {
                        AudioId = "AudioID-123",
                        LanguageId = Languages.English.ToString(),
                        Native = "cat",
                        Spoken = "cat"
                    },
                    new TokenDto {
                        AudioId = "AudioID-321",
                        LanguageId = Languages.Bengoli.ToString(),
                        Native = "???",
                        Spoken = "???"
                    }
                }
            };

            var translation = Converter.Convert(dto, Languages.English.ToString(), Languages.Bengoli.ToString());

            Assert.That(translation.Reference, Is.Not.Null);
            Assert.That(translation.Reference.Label, Is.EqualTo(dto.Label));
            Assert.That(translation.Reference.Level, Is.EqualTo(level));
            Assert.That(translation.Reference.Categories, Is.EquivalentTo(new[] {
                new Category("pet"), new Category("animal")
            }));
            Assert.That(translation.Reference.PictureId, Is.EqualTo(ResourceId.Create(dto.PictureId)));
            Assert.That(translation.Reference.TokenType, Is.EqualTo(tokenType));

            Assert.That(translation.From, Is.Not.Null);
            Assert.That(translation.From.AudioId, Is.EqualTo(ResourceId.Create("AudioID-123")));
            Assert.That(translation.From.Language, Is.EqualTo(Languages.English));
            Assert.That(translation.From.Native, Is.EqualTo("cat"));
            Assert.That(translation.From.Spoken, Is.EqualTo("cat"));

            Assert.That(translation.To, Is.Not.Null);
            Assert.That(translation.To.AudioId, Is.EqualTo(ResourceId.Create("AudioID-321")));
            Assert.That(translation.To.Language, Is.EqualTo(Languages.Bengoli));
            Assert.That(translation.To.Native, Is.EqualTo("???"));
            Assert.That(translation.To.Spoken, Is.EqualTo("???"));
        }

        [Test]
        public void Convert_TranslationDto_invalid_dto()
        {
            var dto = new TranslationDto
            {
                Label = "cat",
                Level = LanguageLevel.A1.ToString(),
                Categories = new[] { "pet", "animal" },
                PictureId = "PicID-123",
                TokenType = TokenType.Noun.ToString(),
                Tokens = new[] {
                    new TokenDto {
                        AudioId = "AudioID-123",
                        LanguageId = Languages.English.ToString(),
                        Native = "cat",
                        Spoken = "cat"
                    }
                }
            };

            Assert.Throws<InvalidTranslationException>(()
                => Converter.Convert(dto, Languages.English.ToString(), Languages.Bengoli.ToString()));
        }
    }
}