using Bhasha.Domain;
using Bhasha.Grains;
using Microsoft.AspNetCore.Components;
using Orleans;

namespace Bhasha.Web.Shared.Components;

public partial class SelectChapter : ComponentBase
{
    [Inject] public IClusterClient ClusterClient { get; set; } = default!;

    [Parameter] public Action<DisplayedSummary> OnSelection { get; set; }
    [Parameter] public string UserId { get; set; }
    [Parameter] public LangKey Languages { get; set; }

    private IList<DisplayedSummary> _chapters = new List<DisplayedSummary>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var grain = ClusterClient.GetGrain<IStudentGrain>(UserId);
        var summaries = await grain.GetSummaries(Languages);

        foreach (var chapter in summaries)
        {
            _chapters.Add(chapter);
        }

        StateHasChanged();
    }
}

