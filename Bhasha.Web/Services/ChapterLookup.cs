using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services;

public class ChapterLookup : IChapterLookup
{
    private readonly IRepository<Chapter> _repository;

    public ChapterLookup(IRepository<Chapter> repository)
	{
        _repository = repository;
    }

    public async IAsyncEnumerable<Chapter> GetChapters(int level)
    {
        var chapters = await _repository.Find(chapter => chapter.RequiredLevel == level);

        foreach (var chapter in chapters)
        {
            yield return chapter;
        }
    }
}

