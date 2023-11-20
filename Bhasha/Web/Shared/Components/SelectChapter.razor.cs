using Bhasha.Domain;
using Bhasha.Services;
using Bhasha.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class SelectChapter : ComponentBase
{
    [Inject] 
    public IStudyingService StudyingService { get; set; } = default!;

    [Parameter] public Func<DisplayedSummary, Task> OnSelection { get; set; } = default!;
    [Parameter] public string UserId { get; set; } = default!;
    [Parameter] public ProfileKey Languages { get; set; } = default!;

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