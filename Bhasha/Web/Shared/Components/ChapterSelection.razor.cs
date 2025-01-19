using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class ChapterSelection : ComponentBase
{
    [Inject] public required IChapterSummariesProvider ChapterSummariesProvider { get; set; }

    [Parameter] public required Func<Summary, Task> ChapterSelected { get; set; }
    [Parameter] public Language Language { get; set; } = Language.Reference;
    [Parameter] public int? RequiredLevel { get; set; }
    
    private IReadOnlyList<Summary> Chapters { get; set; } = new List<Summary>();
    private int PageOffset => 0;
    private int PageSize => 10;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Chapters = await ChapterSummariesProvider.GetSummaries(PageOffset, PageSize, Language);

        await InvokeAsync(StateHasChanged);
    }
}