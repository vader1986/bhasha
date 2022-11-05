using System.Linq.Expressions;

namespace Bhasha.Web.Interfaces;

public interface IRepository<T>
{
	/// <summary>
	/// Adds a new instance of <typeparamref name="T"/> to the repository.
	/// </summary>
	/// <param name="item">Item to add to the repository</param>
	/// <returns>The item which has been added to the repository.</returns>
	Task<T> Add(T item);

	/// <summary>
	/// Queries an item of the repository by its ID. 
	/// </summary>
	/// <param name="id">ID of the item to look for</param>
	/// <returns>The item with the specified ID or <c>null</c> if not available.</returns>
	Task<T?> Get(Guid id);

	Task<T[]> GetMany(params Guid[] ids);
	Task<T[]> Find(Expression<Func<T, bool>> query);
	Task<T[]> Find(Expression<Func<T, bool>> query, int samples);
	Task<bool> Remove(Guid id);
	Task<bool> Update(Guid id, T item);
}