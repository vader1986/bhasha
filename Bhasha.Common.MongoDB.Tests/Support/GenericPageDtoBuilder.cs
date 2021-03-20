using System;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Tests.Support;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class GenericPageDtoBuilder
    {
        public static GenericPageDto Build(Guid? tokenId = default)
        {
            return new GenericPageDto
            {
                TokenId = tokenId ?? Guid.NewGuid(),
                PageType = Rnd.Create.Choose(Enum.GetNames<PageType>())
            };
        }
    }
}
