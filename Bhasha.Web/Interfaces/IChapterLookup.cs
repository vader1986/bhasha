using Bhasha.Web.Domain;

namespace Bhasha.Web.Interfaces;

public interface IChapterLookup
{
    IAsyncEnumerable<Chapter> GetChapters(int level);
}

