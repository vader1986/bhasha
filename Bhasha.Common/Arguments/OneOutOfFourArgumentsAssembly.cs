using System;
using System.Collections.Generic;
using System.Linq;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Arguments
{
    public class OneOutOfFourArgumentsAssembly : IAssembleArguments
    {
        private static OneOutOfFourArguments.Option ConvertToOption(TranslatedExpression expression)
        {
            return new OneOutOfFourArguments.Option(expression.Native, $"{expression.Native} ({expression.Spoken})");
        }

        public object Assemble(IEnumerable<TranslatedExpression> translations, Guid expressionId)
        {
            var options = translations
                .Where(x => x.Expression.Id != expressionId)
                .Random(3)
                .Append(translations.First(x => x.Expression.Id == expressionId))
                .Select(ConvertToOption)
                .ToArray();

            options.Shuffle();

            return new OneOutOfFourArguments(options);
        }
    }
}
