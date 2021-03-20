using System;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Tests.Support;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class UserDtoBuilder
    {
        public static UserDto Build()
        {
            return new UserDto
            {
                Id = Guid.NewGuid(),
                UserName = Rnd.Create.NextString(),
                Email = Rnd.Create.NextString()
            };
        }
    }
}
