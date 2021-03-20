using System;
using System.Collections.Generic;
using System.Linq;
using Bhasha.Common.Extensions;
using Bhasha.Common.Services;

namespace Bhasha.Common.Arguments
{
    public class OneOutOfFourArgumentsAssembly : IAssembleArguments
    {
        public object Assemble(IEnumerable<Translation> translations, Guid tokenId)
        {
            var options = translations
                .Where(x => x.TokenId != tokenId)
                .Random(3)
                .Append(translations.First(x => x.TokenId == tokenId))
                .Select(x => x.Native)
                .ToArray();

            options.Shuffle();

            return new OneOutOfFourArguments(options);
        }

        public bool Supports(PageType pageType)
        {
            return pageType == PageType.OneOutOfFour;
        }
    }
}
