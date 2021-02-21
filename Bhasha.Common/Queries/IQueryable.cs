using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common.Queries
{
    public interface IQueryable<TProduct, TQuery>
    {
        Task<IEnumerable<TProduct>> Query(TQuery query);
    }
}
