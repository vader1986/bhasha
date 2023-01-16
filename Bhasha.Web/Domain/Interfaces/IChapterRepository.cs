namespace Bhasha.Web.Domain.Interfaces;

public interface IChapterRepository
{
    Task<Chapter> Upsert(Chapter chapter);
    ValueTask<Chapter?> Find(Guid chapterId);
    IAsyncEnumerable<Chapter> GetChapters(int level);
}

