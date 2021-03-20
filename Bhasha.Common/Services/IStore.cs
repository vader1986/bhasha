using System;
using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    public interface IStore<TProduct>
    {
        Task<TProduct> Add(TProduct product);
        Task<TProduct> Get(Guid id);
        Task<int> Remove(TProduct product);
        Task Replace(TProduct product);
    }
}
