namespace Bhasha.Web.Interfaces
{
	public interface IFactory<TProduct>
	{
		TProduct Create();
	}
}