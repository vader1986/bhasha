using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common.Queries
{
    public interface IListable<TValue>
    {
        Task<IEnumerable<TValue>> List();
    }
}
