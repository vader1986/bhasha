using Bhasha.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class ChapterPage : ComponentBase
{
    [Parameter] public required DisplayedChapter Chapter { get; set; }
    [Parameter] public required DisplayedPage Page { get; set; }
    [Parameter] public required Func<Translation, Task> Submit { get; set; }

    internal DisplayedPage<T> GetPage<T>()
    {
        return (DisplayedPage<T>)Page;
    }
}

