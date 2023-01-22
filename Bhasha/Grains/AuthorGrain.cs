using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Grains;

public interface IAuthorGrain : IGrainWithStringKey
{
    /// <summary>
    /// Adds or updates an existing translation.
    /// </summary>
    /// <param name="language">Language of the specified text</param>
    /// <param name="expressionId">ID of the expression the text refers to</param>
    /// <param name="text">Translated text</param>
    /// <param name="spoken">Spoken version of the text to describe correct pronounciation (optional)</param>
    /// <param name="audioId">ID of the audio file assiciated with the text (optional)</param>
    Task AddOrUpdateTranslation(string language, Guid expressionId, string text, string? spoken = default, string? audioId = default);

    /// <summary>
    /// Gets an existing expression ID for the specified <paramref name="expression"/>
    /// or adds a new expression and its <see cref="Translation"/> in <see cref="Language.Reference"/>.
    /// <param name="expression">Expression in the <see cref="Language.Reference"/> to look for.</param>
    /// <returns>The <see cref="Expression.Id"/> for the specified <paramref name="expression"/>.</returns>
    Task<Guid> GetOrAddExpressionId(string expression);

    /// <summary>
    /// Adds a new, language-independent chapter or updates an existing one.
    /// </summary>
    /// <param name="chapter">Chapter to add or update.</param>
    Task AddOrUpdateChapter(Chapter chapter);
}

public class AuthorGrain : Grain, IAuthorGrain
{
    private readonly IChapterRepository _chapterRepository;
    private readonly IExpressionRepository _expressionRepository;
    private readonly ITranslationRepository _translationRepository;

    public AuthorGrain(
        IChapterRepository chapterRepository,
        IExpressionRepository expressionRepository,
        ITranslationRepository translationRepository)
	{
        _chapterRepository = chapterRepository;
        _expressionRepository = expressionRepository;
        _translationRepository = translationRepository;
    }

    public async Task AddOrUpdateChapter(Chapter chapter)
    {
        await _chapterRepository.AddOrReplace(chapter);
    }

    public async Task AddOrUpdateTranslation(string language, Guid expressionId, string text, string? spoken = null, string? audioId = null)
    {
        var translation = Translation.Create(expressionId, language, text) with
        {
            Spoken = spoken,
            AudioId = audioId
        };

        await _translationRepository.AddOrUpdate(translation);
    }

    public async Task<Guid> GetOrAddExpressionId(string expression)
    {
        var reference = await _translationRepository.Find(expression);

        if (reference != null)
        {
            return reference.ExpressionId;
        }

        var expr = await _expressionRepository.Add(Expression.Create(1));
        var translation = Translation.Create(expr.Id, Language.Reference, expression);

        await _translationRepository.AddOrUpdate(translation);

        return expr.Id;
    }
}

