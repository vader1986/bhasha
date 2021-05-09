using System;
using System.Collections.Generic;

namespace Bhasha.Common.Arguments
{
    public interface IAssembleArguments
    {
        object Assemble(IEnumerable<TranslatedExpression> translations, Guid expressionId);
    }
}
