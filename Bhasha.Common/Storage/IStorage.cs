#nullable enable
using System.Collections.Generic;

namespace Bhasha.Common.Storage
{
    public interface IStorage<TEntity, TProp, TValue>
    {
        IEnumerable<TEntity> Query(IDictionary<TProp, TValue> query);
    }
}
