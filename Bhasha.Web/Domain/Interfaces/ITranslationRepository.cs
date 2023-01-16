namespace Bhasha.Web.Domain.Interfaces;

public interface ITranslationRepository
{
    ValueTask<Translation?> Find(Guid expressionId, Language language);
    ValueTask<Translation?> Find(string expression);
    Task AddOrUpdate(Translation translation);
    Task<Translation> Get(Guid translationId);
}

