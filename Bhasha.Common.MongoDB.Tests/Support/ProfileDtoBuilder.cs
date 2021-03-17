using System;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Tests.Support;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class ProfileDtoBuilder
    {
        private static readonly string[] Languages = Language.Supported.Keys.ToArray();

        public static ProfileDto Build(Guid? userId = default)
        {
            return new ProfileDto {
                Id = Guid.NewGuid(),
                From = Rnd.Create.Choose(Languages),
                To = Rnd.Create.Choose(Languages),
                Level = Rnd.Create.Next(1, 10),
                UserId = userId ?? Guid.NewGuid()                
            };
        }
    }
}
