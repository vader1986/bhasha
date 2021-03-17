using System;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class PageDtoBuilder
    {
        public static PageDto Build()
        {
            return new PageDto {
                TokenId = Guid.NewGuid(),
                PageType = PageType.OneOutOfFour.ToString(),
                Arguments = Rnd.Create.NextString()
            };
        }
    }
}
