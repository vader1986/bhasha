using Bhasha.Domain;
using Bhasha.Grains;
using Microsoft.AspNetCore.Components;
using Orleans.Streams;

namespace Bhasha.Web.Pages;

public partial class StudentPage : UserPage, IAsyncObserver<Profile>, IDisposable
{
    [Inject] public IClusterClient ClusterClient { get; set; } = default!;

    private Profile? _selectedProfile;
    private LangKey? _selectedLanguages;
    private DisplayedChapter? _selectedChapter;
    private DisplayedPage? _selectedPage;
    private string? _error;
    private StreamSubscriptionHandle<Profile>? _subscription;

    private bool DisplayProfileSelection => _selectedProfile == null || _selectedLanguages == null;
    private bool DisplayChapterSelection => !DisplayProfileSelection && _selectedChapter == null;
    private bool DisplayPage => _selectedProfile != null && _selectedChapter != null && _selectedPage != null;

    private readonly IList<Profile> _profiles = new List<Profile>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var grain = ClusterClient.GetGrain<IStudentGrain>(UserId);
        var profiles = await grain.GetProfiles();

        foreach (var profile in profiles)
        {
            _profiles.Add(profile);
        }

        var streamProvider = ClusterClient.GetStreamProvider(Orleans.StreamProvider);
        var stream = streamProvider.GetStream<Profile>(Orleans.Streams.UserProfile, UserId);

        _subscription = await stream.SubscribeAsync(this);

        StateHasChanged();
    }

    internal void OnSelectProfile(Profile profile)
    {
        _selectedProfile = profile;
        _selectedLanguages = profile.Key.LangId;

        StateHasChanged();
    }

    internal async void OnSubmit(Translation translation)
    {
        try
        {
            if (_selectedLanguages == null)
                throw new InvalidOperationException("No languages selected");

            if (_selectedPage == null)
                throw new InvalidOperationException("No page selected");

            var expressionId = _selectedPage.Word.ExpressionId;
            var userInput = new ValidationInput(_selectedLanguages, expressionId, translation);
            var grain = ClusterClient.GetGrain<IStudentGrain>(UserId);

            await grain.Submit(userInput);
        }
        catch (Exception error)
        {
            _error = error.Message;
            StateHasChanged();
        }
    }

    internal async void OnSelectChapter(DisplayedSummary summary)
    {
        try
        {
            if (_selectedProfile == null)
                throw new InvalidOperationException("No profile selected");

            var key = _selectedProfile.Key.LangId;
            var grain = ClusterClient.GetGrain<IStudentGrain>(UserId);
            var chapter = await grain.SelectChapter(summary.ChapterId, key);
            var profile = await grain.GetProfile(key);

            var selection = profile.CurrentChapter;

            if (selection == null)
                throw new InvalidOperationException("No chapter selected for current profile");

            if (selection.ChapterId != chapter.Id)
                throw new InvalidOperationException("Selected chapter doesn't match profile");

            _selectedChapter = chapter;
            _selectedPage = chapter.Pages[selection.PageIndex];
        }
        catch (Exception error)
        {
            _error = error.Message;
        }
        finally
        {
            StateHasChanged();
        }
    }

    public async Task OnNextAsync(Profile profile, StreamSequenceToken? token = null)
    {
        try
        {
            if (_selectedChapter is null)
                return;

            if (_selectedProfile is null || _selectedProfile.Id != profile.Id)
                return;

            var selection = profile.CurrentChapter;

            if (selection == null)
            {
                _selectedChapter = null;
                _selectedPage = null;
            }
            else
            {
                _selectedProfile = profile;
                _selectedPage = _selectedChapter.Pages[selection.PageIndex];
            }
        }
        catch (Exception error)
        {
            _error = error.Message;
        }

        await InvokeAsync(StateHasChanged);
    }

    public Task OnErrorAsync(Exception ex)
    {
        _error = ex.Message;

        return InvokeAsync(StateHasChanged);
    }

    public Task OnCompletedAsync()
    {
        return Task.CompletedTask;
    }

    public async void Dispose()
    {
        if (_subscription != null)
        {
            await _subscription.UnsubscribeAsync();
        }
    }
}

