using Bhasha.Domain;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class SelectChapter : ComponentBase
{
    [Inject] 
    public required IStudyingService StudyingService { get; set; }

    [Parameter] public required Func<DisplayedSummary, Task> OnSelection { get; set; }
    [Parameter] public required string UserId { get; set; }
    [Parameter] public required ProfileKey Languages { get; set; }

    private IList<DisplayedSummary> Chapters { get; } = new List<DisplayedSummary>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Chapters.Clear();
        
        var summaries = await StudyingService.GetSummaries(Languages);

        foreach (var chapter in summaries)
        {
            Chapters.Add(chapter);
        }

        await InvokeAsync(StateHasChanged);
    }
}