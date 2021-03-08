using System;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class ChapterDtoBuilder
    {
        public static ChapterDto Build()
        {
            return new ChapterDto {
                Id = Guid.NewGuid(),
                Level = Rnd.Create.Next(1, 10),
                Name = Rnd.Create.NextString(),
                Description = Rnd.Create.NextPhrase(),
                Pages = Enumerable
                    .Range(0, Rnd.Create.Next(1, 5))
                    .Select(_ => PageDtoBuilder.Build())
                    .ToArray(),
                PictureId = Rnd.Create.NextString()
            };
        }
    }
}
