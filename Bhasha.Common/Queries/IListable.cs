using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common.Queries
{
    public interface IListable<TValue>
    {
        ValueTask<IEnumerable<TValue>> List();
    }
}
