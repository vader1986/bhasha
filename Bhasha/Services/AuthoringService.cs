using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Chapter = Bhasha.Domain.Chapter;
using Expression = Bhasha.Domain.Expression;

namespace Bhasha.Services;

public interface IAuthoringService
{
    Task<Expression> GetOrCreateExpression(string text, int level, CancellationToken token = default);
    Task AddOrUpdateTranslation(Translation translation, CancellationToken token= default);
    Task AddOrUpdateChapter(Chapter chapter, CancellationToken token = default);
}

public sealed class AuthoringService(
    IChapterRepository chapterRepository,
    ITranslationRepository translationRepository,
    ITranslationProvider translationProvider,
    IExpressionRepository expressionRepository) : IAuthoringService
{
    public async Task<Expression> GetOrCreateExpression(string text, int level, CancellationToken token = default)
    {
        var reference = await translationRepository.Find(text, Language.Reference, token);

        if (reference != null)
        {
            return reference.Expression;
        }

        var expression = await expressionRepository
            .Add(Expression.Create(level) with
            {
                Labels = [text]
            }, token);
        
        var translation = await translationRepository
            .AddOrUpdate(Translation.Create(expression, Language.Reference, text), token);

        await translationProvider
            .AddOrUpdate(translation, token);
        
        return expression;
    }

    public async Task AddOrUpdateTranslation(Translation translation, CancellationToken token = default)
    {
        var updatedTranslation = await translationRepository
            .AddOrUpdate(translation, token);
        
        await translationProvider
            .AddOrUpdate(updatedTranslation, token);
    }

    public async Task AddOrUpdateChapter(Chapter chapter, CancellationToken token = default)
    {
        await chapterRepository.AddOrUpdate(chapter, token);
    }
}