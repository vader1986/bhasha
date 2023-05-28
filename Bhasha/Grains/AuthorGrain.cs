using Bhasha.Domain;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Grains;

public interface IAuthorGrain : IGrainWithStringKey
{
    /// <summary>
    /// Gets an existing expression ID for the specified <paramref name="expression"/>
    /// or adds a new expression and its <see cref="Translation"/> in <see cref="Language.Reference"/>.
    /// <param name="expression">Expression in the <see cref="Language.Reference"/> to look for.</param>
    /// <returns>The <see cref="Expression.Id"/> for the specified <paramref name="expression"/>.</returns>
    Task<Guid> GetOrAddExpressionId(string expression);

    /// <summary>
    /// Adds or updates an existing translation.
    /// </summary>
    /// <param name="translation">New or updated translation</param>
    Task AddOrUpdateTranslation(Translation translation);

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

    public AuthorGrain(IChapterRepository chapterRepository, IExpressionRepository expressionRepository, ITranslationRepository translationRepository)
	{
        _chapterRepository = chapterRepository;
        _expressionRepository = expressionRepository;
        _translationRepository = translationRepository;
    }

    public async Task AddOrUpdateChapter(Chapter chapter)
    {
        await _chapterRepository.AddOrReplace(chapter);
    }

    public async Task AddOrUpdateTranslation(Translation translation)
    {
        await _translationRepository.AddOrReplace(translation);
    }

    public async Task<Guid> GetOrAddExpressionId(string expression)
    {
        var reference = await _translationRepository.Find(expression, Language.Reference);

        if (reference != null)
        {
            return reference.ExpressionId;
        }

        var expr = await _expressionRepository.Add(Expression.Create(1));
        var translation = Translation.Create(expr.Id, Language.Reference, expression);

        await _translationRepository.AddOrReplace(translation);

        return expr.Id;
    }
}

