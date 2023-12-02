namespace Bhasha.Domain.Interfaces;

public interface IChapterRepository
{
    Task<Chapter> AddOrUpdate(Chapter chapter, CancellationToken token);
    Task<Chapter?> FindById(int chapterId, CancellationToken token);
    Task<IEnumerable<Chapter>> FindByLevel(int level, CancellationToken token);
}

