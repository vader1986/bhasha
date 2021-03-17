using System;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class TranslationDtoBuilder
    {
        private static string[] Languages = Language.Supported.Keys.ToArray();

        public static TranslationDto Build(Guid? tokenId = default)
        {
            return new TranslationDto
            {
                TokenId = tokenId ?? Guid.NewGuid(),
                Language = Rnd.Create.Choose(Languages),
                Native = Rnd.Create.NextString(),
                Spoken = Rnd.Create.NextString(),
                AudioId = Rnd.Create.NextString()
            };
        }
    }
}
