namespace Bhasha.Shared.Domain.Interfaces;

public interface IChapterRepository
{
    Task<Chapter> AddOrReplace(Chapter chapter);
    ValueTask<Chapter?> FindById(Guid chapterId);
    IAsyncEnumerable<Chapter> FindByLevel(int level);
}

