using System;
using System.Threading.Tasks;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Interface for database storage.
    /// </summary>
    /// <typeparam name="TProduct">Type of product to store in the DB.</typeparam>
    public interface IStore<TProduct> where TProduct: class, IEntity, ICanBeValidated
    {
        /// <summary>
        /// Adds a new product into the database. Ignore the product if there's
        /// already a product with the same ID available in the database. 
        /// </summary>
        /// <param name="product">Product to add to the storage.</param>
        /// <returns>The new product with updated ID.</returns>
        Task<TProduct> Add(TProduct product);

        Task<TProduct?> Get(Guid id);

        Task<int> Remove(Guid id);

        Task Replace(TProduct product);
    }
}
