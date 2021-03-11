using System;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class PageDtoBuilder
    {
        private static string[] Languages = Language.Supported.Keys.ToArray();

        public static PageDto Build()
        {
            return new PageDto {
                TokenId = Guid.NewGuid(),
                Language = Rnd.Create.Choose(Languages),
                PageType = PageType.ChooseSolution.ToString(),
                Arguments = Rnd.Create.NextStrings().ToArray()
            };
        }
    }
}
