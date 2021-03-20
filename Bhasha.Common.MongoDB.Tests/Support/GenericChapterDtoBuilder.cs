using System;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Tests.Support;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class GenericChapterDtoBuilder
    {
        public static GenericChapterDto Build(params Guid[] tokenIds)
        {
            return new GenericChapterDto
            {
                Id = Guid.NewGuid(),
                Level = Rnd.Create.Next(),
                Name = Rnd.Create.NextString(),
                Description = Rnd.Create.NextPhrase(),
                PictureId = Rnd.Create.NextString(),
                Pages = tokenIds
                    .Select(x => GenericPageDtoBuilder.Build(x))
                    .ToArray()
            };
        }
    }
}
