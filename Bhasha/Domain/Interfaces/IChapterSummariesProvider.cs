namespace Bhasha.Domain.Interfaces;

public interface IChapterSummariesProvider
{
    Task<IReadOnlyList<Summary>> GetSummaries(int level, ProfileKey key, CancellationToken token = default);
}

