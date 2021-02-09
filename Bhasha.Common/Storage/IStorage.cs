#nullable enable
using System.Collections.Generic;

namespace Bhasha.Common.Storage
{
    public interface IStorage
    {
        IEnumerable<Translation> Query(QueryParams query);
    }
}
