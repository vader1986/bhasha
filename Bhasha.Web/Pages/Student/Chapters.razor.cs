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

    private ChapterDescription[] _chapters = Array.Empty<ChapterDescription>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _chapters = await ChapterProvider.GetChapters(ProfileId);
        await LoadStream();
    }

    internal async Task OnSelectChapter(ChapterDescription chapter)
    {
        var profile = await ProgressManager.StartChapter(ProfileId, chapter.ChapterId);

        var chapterId = profile.Progress.ChapterId;
        var pageIndex = profile.Progress.PageIndex;

        NavigationManager.NavigateTo($"pages/{ProfileId}/{chapterId}/{pageIndex}");
    }

    #region TODO REMOVE
    [Inject]
    public IClusterClient ClusterClient { get; set; }

    private async Task LoadStream()
    {
        try
        {
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

    public async Task OnNextAsync(int item, StreamSequenceToken token = null)
    {
        _items.Add($"{item}");
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

