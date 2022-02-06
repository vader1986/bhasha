using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces
{
	public interface IExpressionManager
	{
		Task<Expression> AddOrUpdate(Expression expression);
	}
}

