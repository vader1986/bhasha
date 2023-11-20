using Bhasha.Shared.Domain;

namespace Bhasha.Domain.Interfaces;

public interface IChapterSummariesProvider
{
    Task<IReadOnlyList<Summary>> GetSummaries(int level, ProfileKey key);
}

