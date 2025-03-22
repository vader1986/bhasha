namespace Bhasha.Domain.Interfaces;

public interface ITranslationProvider
{
    Task<Translation?> Find(int expressionId, string language, CancellationToken token = default);
    
    Task AddOrUpdate(Translation translation, CancellationToken token = default);
}