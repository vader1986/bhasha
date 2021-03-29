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
                NameId = Guid.NewGuid(),
                DescriptionId = Guid.NewGuid(),
                Pages = tokenIds
                    .Select(x => GenericPageDtoBuilder.Build(x))
                    .ToArray()
            };
        }
    }
}
