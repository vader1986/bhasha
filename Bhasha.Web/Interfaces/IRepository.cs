using System.Linq.Expressions;

namespace Bhasha.Web.Interfaces
{
	public interface IRepository<T>
	{
		Task<T> Add(T item);
		Task<T?> Get(Guid id);
		Task<T[]> GetMany(params Guid[] ids);
		Task<T[]> Find(Expression<Func<T, bool>> query);
		Task<bool> Remove(Guid id);
		Task<bool> Update(Guid id, T item);
	}
}