namespace Bhasha.Domain.Interfaces;

public interface IFactory<TProduct>
{
	TProduct Create();
}