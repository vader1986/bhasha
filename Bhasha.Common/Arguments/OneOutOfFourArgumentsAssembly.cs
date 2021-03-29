using System;
using System.Collections.Generic;
using System.Linq;
using Bhasha.Common.Extensions;

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
                .Select(x => $"{x.Native} ({x.Spoken})")
                .ToArray();

            options.Shuffle();

            return new OneOutOfFourArguments(options);
        }
    }
}
