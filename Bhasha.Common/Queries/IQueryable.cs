using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common.Queries
{
    public interface IQueryable<TProduct, TQuery> where TQuery : IQuery
    {
        ValueTask<IEnumerable<TProduct>> Query(TQuery query);
    }
}
