using Bhasha.Web.Domain;
using Orleans;

namespace Bhasha.Web.Grains;

public interface IDisplayChapterGrain : IGrainWithStringKey
{
    ValueTask<DisplayedChapter> Display();
}
