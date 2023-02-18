using Bhasha.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class ChapterPage : ComponentBase
{
    [Parameter] public DisplayedChapter Chapter { get; set;}
    [Parameter] public DisplayedPage Page { get; set; }
    [Parameter] public Action<Translation> Submit { get; set; }

    internal DisplayedPage<T> GetPage<T>()
    {
        return (DisplayedPage<T>)Page;
    }
}

