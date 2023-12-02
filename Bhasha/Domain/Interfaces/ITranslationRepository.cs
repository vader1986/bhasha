namespace Bhasha.Domain.Interfaces;

public interface ITranslationRepository
{
    Task<Translation?> Find(int expressionId, string language, CancellationToken token = default);
    Task<Translation?> Find(string text, string language, CancellationToken token = default);
    Task<Translation> AddOrUpdate(Translation translation, CancellationToken token = default);
    Task<Translation> Get(int translationId, CancellationToken token = default);
}

