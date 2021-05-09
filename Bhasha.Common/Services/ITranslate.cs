using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    /// <summary>
    /// Defines a translation service which translates objects of type <typeparamref name="TObject"/>
    /// into translations of type <typeparamref name="TTranslation"/>.
    /// </summary>
    /// <typeparam name="TObject">Type of object to translate.</typeparam>
    /// <typeparam name="TTranslation">Result type for translations.</typeparam>
    /// <typeparam name="TLanguage">Type of language.</typeparam>
    public interface ITranslate<TObject, TTranslation, TLanguage>
        where TTranslation : class
    {
        /// <summary>
        /// Translates the specified <paramref name="object"/> into the specified
        /// <paramref name="language"/>.
        /// </summary>
        /// <param name="object">Object to translate.</param>
        /// <param name="language">Target language.</param>
        /// <returns>A new instance of <typeparamref name="TTranslation"/> or
        /// <c>null</c> if data is missing to translate the object.</returns>
        Task<TTranslation?> Translate(TObject @object, TLanguage language);
    }

    /// <summary>
    /// Defines a translation service which translates objects of type <typeparamref name="TObject"/>
    /// into translations of type <typeparamref name="TTranslation"/>.
    /// </summary>
    /// <typeparam name="TObject">Type of object to translate.</typeparam>
    /// <typeparam name="TTranslation">Result type for translations.</typeparam>
    public interface ITranslate<TObject, TTranslation> : ITranslate<TObject, TTranslation, Language>
        where TTranslation : class
    {
    }
}
