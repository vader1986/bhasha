using System;
using System.Collections.Generic;

namespace Bhasha.Common.Services
{
    public interface IAssembleArguments : ISupportPageType
    {
        object Assemble(IEnumerable<Translation> translations, Guid tokenId);
    }
}
