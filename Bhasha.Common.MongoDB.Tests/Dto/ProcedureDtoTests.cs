using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Exceptions;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Dto
{
    [TestFixture]
    public class ProcedureDtoTests
    {
        [Test]
        public void ToProcedure_dto_with_invalid_values()
        {
            var dto = new ProcedureDto {
                Support = null
            };

            Assert.Throws<InvalidProcedureException>(() => dto.ToProcedure());
        }

        [Test]
        public void ToProcedure_dto_without_tutorial()
        {
            var dto = new ProcedureDto {
                ProcedureId = "ID-123",
                Description = "my description",
                Support = new string[0],
                Tutorial = new [] { "TutorialID-123" }
            };

            var procedure = dto.ToProcedure();

            Assert.That(procedure.Audio, Is.Null);
            Assert.That(procedure.Id, Is.EqualTo(new ProcedureId(dto.ProcedureId)));
            Assert.That(procedure.Description, Is.EqualTo(dto.Description));
            Assert.That(procedure.Support, Is.EquivalentTo(new string[0]));
            Assert.That(procedure.Tutorial, Is.EquivalentTo(new[] { ResourceId.Create(dto.Tutorial[0]) }));
        }

        [Test]
        public void ToProcedure_dto_without_audio()
        {
            var dto = new ProcedureDto
            {
                ProcedureId = "ID-123",
                Description = "my description",
                Support = new string[0],
                AudioId = "AudioID-123"
            };

            var procedure = dto.ToProcedure();

            Assert.That(procedure.Tutorial, Is.EquivalentTo(new ResourceId[0]));
            Assert.That(procedure.Audio, Is.EqualTo(ResourceId.Create(dto.AudioId)));
            Assert.That(procedure.Id, Is.EqualTo(new ProcedureId(dto.ProcedureId)));
            Assert.That(procedure.Description, Is.EqualTo(dto.Description));
            Assert.That(procedure.Support, Is.EquivalentTo(new string[0]));
        }

        [Test]
        public void ToProcedure_dto_with_custom_token_type([Values]TokenType supportedToken)
        {
            var dto = new ProcedureDto
            {
                ProcedureId = "ID-123",
                Description = "my description",
                Support = new[] { supportedToken.ToString() },
                AudioId = "AudioID-123",
                Tutorial = new[] { "TutorialID-123" }
            };

            var procedure = dto.ToProcedure();

            Assert.That(procedure.Audio, Is.EqualTo(ResourceId.Create(dto.AudioId)));
            Assert.That(procedure.Id, Is.EqualTo(new ProcedureId(dto.ProcedureId)));
            Assert.That(procedure.Description, Is.EqualTo(dto.Description));
            Assert.That(procedure.Support, Is.EquivalentTo(new[] { supportedToken }));
            Assert.That(procedure.Tutorial, Is.EquivalentTo(new[] { ResourceId.Create(dto.Tutorial[0]) }));
        }
    }
}