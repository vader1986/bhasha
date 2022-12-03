using Bhasha.Web.Domain;
using Bhasha.Web.Grains;
using Bhasha.Web.Interfaces;
using Microsoft.AspNetCore.Components;
using Orleans;
using Orleans.Streams;

namespace Bhasha.Web.Pages.Student;

public partial class Chapters : UserPage, IAsyncObserver<int>
{
    [Inject] public IProgressManager ProgressManager { get; set; } = default!;
	[Inject] public IChapterProvider ChapterProvider { get; set; } = default!;
	[Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public Guid ProfileId { get; set; }

    private DisplayedSummary[] _chapters = Array.Empty<DisplayedSummary>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _chapters = await ChapterProvider.GetChapters(ProfileId);
        await LoadStream();
    }

    internal async Task OnSelectChapter(DisplayedSummary chapter)
    {
        var profile = await ProgressManager.StartChapter(ProfileId, chapter.ChapterId);

        var currentChapter = profile.CurrentChapter;
        if (currentChapter != null)
        {
            NavigationManager.NavigateTo($"pages/{ProfileId}/{currentChapter.ChapterId}/{currentChapter.PageIndex}");
        }
    }

    #region TODO REMOVE
    [Inject]
    public IClusterClient? ClusterClient { get; set; }

    private async Task LoadStream()
    {
        try
        {
            if (ClusterClient == null)
                return;

            var grain = ClusterClient.GetGrain<IFakeGrain>("123");
            var shit = await grain.CreateShit();
            _items.Add($"{shit}");

            var streamProvider = ClusterClient.GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<int>(Guid.Empty, "GRAINKEY");
            await stream.SubscribeAsync(this);

            await grain.CreateRandomShit();
            await grain.CreateRandomShit();
            await grain.CreateRandomShit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public Task OnNextAsync(int item, StreamSequenceToken token)
    {
        _items.Add($"{item}");
        return Task.CompletedTask;
    }

    public Task OnCompletedAsync()
    {
        throw new NotImplementedException();
    }

    public Task OnErrorAsync(Exception ex)
    {
        throw new NotImplementedException();
    }

    private readonly List<string> _items = new List<string>();

    #endregion
}

