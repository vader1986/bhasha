using System;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Tests.Support;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class ProfileDtoBuilder
    {
        public static ProfileDto Build(string userId = default)
        {
            return new ProfileDto {
                Id = Guid.NewGuid(),
                From = Language.English,
                To = Language.Bengali,
                Level = Rnd.Create.Next(1, 10),
                UserId = userId ?? Rnd.Create.NextString()
            };
        }
    }
}
