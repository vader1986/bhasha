using System;
using System.Collections.Generic;

namespace Bhasha.Common.Arguments
{
    public interface IAssembleArguments : ISupportPageType
    {
        object Assemble(IEnumerable<Translation> translations, Guid tokenId);
    }
}
