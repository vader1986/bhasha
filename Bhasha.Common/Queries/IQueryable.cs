using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common.Queries
{
    public interface IQueryable<TProduct, TQuery>
    {
        ValueTask<IEnumerable<TProduct>> Query(TQuery query);
    }
}
