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
    private readonly ITranslationRepository _translationRepository;
    private readonly IExpressionRepository _expressionRepository;

    public AuthoringService(IChapterRepository chapterRepository, ITranslationRepository translationRepository, IExpressionRepository expressionRepository)
    {
        _chapterRepository = chapterRepository;
        _translationRepository = translationRepository;
        _expressionRepository = expressionRepository;
    }
    
    public async Task<Expression> GetOrCreateExpression(string text, int level, CancellationToken token = default)
    {
        var reference = await _translationRepository.Find(text, Language.Reference, token);

        if (reference != null)
        {
            return reference.Expression;
        }

        var expression = await _expressionRepository
            .Add(Expression.Create(level) with
            {
                Labels = new[] { text }
            }, token);
        
        await _translationRepository
            .AddOrUpdate(Translation.Create(expression, Language.Reference, text), token);

        return expression;
    }

    public async Task AddOrUpdateTranslation(Translation translation, CancellationToken token = default)
    {
        await _translationRepository.AddOrUpdate(translation, token);
    }

    public async Task AddOrUpdateChapter(Chapter chapter, CancellationToken token = default)
    {
        await _chapterRepository.AddOrUpdate(chapter, token);
    }
}