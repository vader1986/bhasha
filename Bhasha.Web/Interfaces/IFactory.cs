namespace Bhasha.Web.Interfaces
{
	public interface IFactory<TProduct>
	{
		TProduct Create();
	}

	public interface IFactory<TConfig, TProduct>
	{
		TProduct Create(TConfig config);
	}
}