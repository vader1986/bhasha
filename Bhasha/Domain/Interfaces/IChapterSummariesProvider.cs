namespace Bhasha.Domain.Interfaces;

public interface IChapterSummariesProvider
{
    Task<IReadOnlyList<Summary>> GetSummaries(int level, Language language, CancellationToken token = default);
    Task<IReadOnlyList<Summary>> GetSummaries(int offset, int batchSize, Language language, CancellationToken token = default);
}

