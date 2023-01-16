namespace Bhasha.Web.Domain.Interfaces;

public interface IFactory<TProduct>
{
	TProduct Create();
}