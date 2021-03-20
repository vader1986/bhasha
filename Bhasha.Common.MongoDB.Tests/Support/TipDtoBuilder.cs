using System;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Tests.Support;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class TipDtoBuilder
    {
        public static TipDto Build()
        {
            return new TipDto
            {
                Id = Guid.NewGuid(),
                ChapterId = Guid.NewGuid(),
                PageIndex = Rnd.Create.Next(),
                Text = Rnd.Create.NextString()
            };
        }
    }
}
