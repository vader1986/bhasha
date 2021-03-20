using System;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Tests.Support;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class TokenDtoBuilder
    {
        public static TokenDto Build(Guid? id = default)
        {
            return new TokenDto {
                Id = id ?? Guid.NewGuid(),
                Label = Rnd.Create.NextString(),
                Level = Rnd.Create.Next(1, 10),
                Cefr = Rnd.Create.Choose(Enum.GetNames(typeof(CEFR))),
                TokenType = Rnd.Create.Choose(Enum.GetNames(typeof(TokenType))),
                Categories = Rnd.Create.NextStrings().ToArray(),
                PictureId = Rnd.Create.NextString()
            };
        }
    }
}
