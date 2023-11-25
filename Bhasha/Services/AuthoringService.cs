using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;

namespace Bhasha.Services;

public interface IAuthoringService
{
    Task<Expression> GetOrCreateExpression(string text, int level, CancellationToken token = default);
    Task AddOrUpdateTranslation(Translation translation, CancellationToken token= default);
    Task AddOrUpdateChapter(Chapter chapter, CancellationToken token = default);
}

public class AuthoringService : IAuthoringService
{
    private readonly IChapterRepository _chapterRepository;
    private readonly IExpressionRepository _expressionRepository;
    private readonly ITranslationRepository _translationRepository;

    public AuthoringService(IChapterRepository chapterRepository, IExpressionRepository expressionRepository, ITranslationRepository translationRepository)
    {
        _chapterRepository = chapterRepository;
        _expressionRepository = expressionRepository;
        _translationRepository = translationRepository;
    }
    
    public async Task<Expression> GetOrCreateExpression(string text, int level, CancellationToken token = default)
    {
        var reference = await _translationRepository.Find(text, Language.Reference);

        if (reference != null)
        {
            return reference.Expression;
        }

        var expression = await _expressionRepository
            .Add(Expression.Create(level));
        
        var translation = Translation
            .Create(expression, Language.Reference, text);

        await _translationRepository
            .AddOrReplace(translation);

        return expression;
    }

    public async Task AddOrUpdateTranslation(Translation translation, CancellationToken token = default)
    {
        await _translationRepository.AddOrReplace(translation);
    }

    public async Task AddOrUpdateChapter(Chapter chapter, CancellationToken token = default)
    {
        await _chapterRepository.AddOrReplace(chapter, token);
    }
}