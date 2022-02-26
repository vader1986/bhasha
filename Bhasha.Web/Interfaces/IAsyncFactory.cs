namespace Bhasha.Web.Interfaces
{
	/// <summary>
    /// Creates a new instance of <typeparamref name="TProduct"/> using the
    /// specifed <typeparamref name="TArg"/>. 
    /// </summary>
    /// <typeparam name="TArg">Configuration type used to create the product</typeparam>
    /// <typeparam name="TProduct">Type of the product</typeparam>
	public interface IAsyncFactory<TArg, TProduct>
	{
        /// <summary>
        /// Creates a new instance of <typeparamref name="TProduct"/> using the
        /// specified <typeparamref name="TArg"/>.
        /// </summary>
        /// <param name="config">Configuration for creating the product</param>
        /// <returns>A new instance of <typeparamref name="TProduct"/>.</returns>
		Task<TProduct> CreateAsync(TArg config);
	}

    /// <summary>
    /// Creates a new instance of <typeparamref name="TProduct"/> using the
    /// specifed arguments <typeparamref name="TArg1"/> and <typeparamref name="TArg2"/>. 
    /// </summary>
    /// <typeparam name="TArg1">First argument for creating the product</typeparam>
    /// <typeparam name="TArg2">Second argument for creating the product</typeparam>
    /// <typeparam name="TProduct">Type of the product</typeparam>
    public interface IAsyncFactory<TArg1, TArg2, TProduct>
	{
        /// <summary>
        /// Creates a new instance of <typeparamref name="TProduct"/> using the
        /// specified arguments.
        /// </summary>
        /// <param name="arg1">First argument for creating the product</param>
        /// <param name="arg2">Second argument for creating the product</param>
        /// <returns>A new instance of <typeparamref name="TProduct"/>.</returns>
		Task<TProduct> CreateAsync(TArg1 arg1, TArg2 arg2);
	}
}

