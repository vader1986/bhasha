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
        public void Convert_PrifleDto_with_invalid_language()
        {
            var dto = ProfileDtoBuilder.Build();
            dto.From = "asdf";

            Assert.Throws<InvalidDtoException>(() => Converter.Convert(dto));
        }


    }
}