using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface ITranslationProvider
{
    /// <summary>
    /// Finds a translation for the specified expression into the specified language.
    /// </summary>
    /// <param name="expressionId">ID for the expression to look for.</param>
    /// <param name="language">Language to find a translation for.</param>
    /// <returns>A translation for the expression into the language or <c>null</c> if not available.</returns>
	Task<Translation?> Find(Guid expressionId, Language language);

	/// <summary>
    /// Find all translation for the specified expressions into the specified language.
    /// </summary>
    /// <param name="language">Language to translate expressions into.</param>
    /// <param name="expressionIds">Expression IDs to find translations for.</param>
    /// <returns>A lookup table for all translations.</returns>
    /// <exception cref="InvalidOperationException">Could not find any of the translations in the database.</exception>
	Task<IDictionary<Guid, Translation>> FindAll(Language language, params Guid[] expressionIds);
}

