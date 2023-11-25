namespace Bhasha.Shared.Domain.Interfaces;

public interface IChapterRepository
{
    Task<Chapter> AddOrReplace(Chapter chapter, CancellationToken token);
    Task<Chapter?> FindById(int chapterId, CancellationToken token);
    IAsyncEnumerable<Chapter> FindByLevel(int level, CancellationToken token);
}

