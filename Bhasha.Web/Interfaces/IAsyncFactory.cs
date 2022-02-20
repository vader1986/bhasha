namespace Bhasha.Web.Interfaces
{
	public interface IAsyncFactory<TArg, TProduct>
	{
		Task<TProduct> CreateAsync(TArg config);
	}

	public interface IAsyncFactory<TArg1, TArg2, TProduct>
	{
		Task<TProduct> CreateAsync(TArg1 arg1, TArg2 arg2);
	}
}

