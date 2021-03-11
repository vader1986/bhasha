using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class LanguageTokenDtoBuilder
    {
        public static LanguageTokenDto Build()
        {
            return new LanguageTokenDto {
                Native = Rnd.Create.NextString(),
                Spoken = Rnd.Create.NextString(),
                AudioId = Rnd.Create.NextString()
            };
        }
    }
}
