using System;
using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    public interface IStore<TProduct> where TProduct: class
    {
        Task<TProduct> Add(TProduct product);
        Task<TProduct?> Get(Guid id);
        Task<int> Remove(TProduct product);
        Task Replace(TProduct product);
    }
}
