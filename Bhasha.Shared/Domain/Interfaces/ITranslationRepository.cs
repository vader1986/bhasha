namespace Bhasha.Shared.Domain.Interfaces;

public interface ITranslationRepository
{
    ValueTask<Translation?> Find(Guid expressionId, Language language);
    ValueTask<Translation?> Find(string expression, Language language);
    Task AddOrReplace(Translation translation);
    Task<Translation> Get(Guid translationId);
}

