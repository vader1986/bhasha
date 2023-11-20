using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;

namespace Bhasha.Services;

public interface IAuthoringService
{
    Task<Guid> GetExpressionId(string text, int level);
    Task AddOrUpdateTranslation(Translation translation);
    Task AddOrUpdateChapter(Chapter chapter);
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
    
    public async Task<Guid> GetExpressionId(string text, int level)
    {
        var reference = await _translationRepository.Find(text, Language.Reference);

        if (reference != null)
        {
            return reference.ExpressionId;
        }

        var expr = await _expressionRepository.Add(Expression.Create(level));
        var translation = Translation.Create(expr.Id, Language.Reference, text);

        await _translationRepository.AddOrReplace(translation);

        return expr.Id;
    }

    public async Task AddOrUpdateTranslation(Translation translation)
    {
        await _translationRepository.AddOrReplace(translation);
    }

    public async Task AddOrUpdateChapter(Chapter chapter)
    {
        await _chapterRepository.AddOrReplace(chapter);
    }
}