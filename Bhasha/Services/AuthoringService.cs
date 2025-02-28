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
        
        await translationRepository
            .AddOrUpdate(Translation.Create(expression, Language.Reference, text), token);

        return expression;
    }

    public async Task AddOrUpdateTranslation(Translation translation, CancellationToken token = default)
    {
        await translationRepository.AddOrUpdate(translation, token);
    }

    public async Task AddOrUpdateChapter(Chapter chapter, CancellationToken token = default)
    {
        await chapterRepository.AddOrUpdate(chapter, token);
    }
}