namespace Bhasha.Shared.Domain.Interfaces;

public interface ITranslationRepository
{
    Task<Translation?> Find(int expressionId, Language language, CancellationToken token = default);
    Task<Translation?> Find(string text, Language language, CancellationToken token = default);
    Task AddOrReplace(Translation translation, CancellationToken token = default);
    Task<Translation> Get(int translationId, CancellationToken token = default);
}

