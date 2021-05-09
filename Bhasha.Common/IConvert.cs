namespace Bhasha.Common
{
    /// <summary>
    /// Interface definition for data converters from <typeparamref name="TInput"/>
    /// to <typeparamref name="TOutput"/>.
    /// </summary>
    /// <typeparam name="TInput">Input type for data to convert.</typeparam>
    /// <typeparam name="TOutput">Conversion output data type.</typeparam>
    public interface IConvert<TInput, TOutput>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> into <typeparamref name="TOutput"/>.
        /// </summary>
        TOutput Convert(TInput input);
    }
}
