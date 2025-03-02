using Bhasha.Domain;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using Profile = Bhasha.Domain.Profile;

namespace Bhasha.Web.Pages;

public partial class StudentPage : UserPage
{
    [Inject] public required IStudyingService StudyingService { get; set; }
    
    private string? _error;
    private Profile? _selectedProfile;
    private DisplayedChapter? _selectedChapter;
    private DisplayedPage? _selectedPage;
    
    private readonly IList<Profile> _profiles = new List<Profile>();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            var profiles = await StudyingService.GetProfiles(UserId);

            foreach (var profile in profiles)
            {
                _profiles.Add(profile);
            }
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task SelectProfileAsync(Profile profile)
    {
        _selectedProfile = profile;

        await InvokeAsync(StateHasChanged);
    }
    
    private async Task SelectChapterAsync(DisplayedSummary summary)
    {
        try
        {
            if (_selectedProfile == null)
                throw new InvalidOperationException("no profile selected");

            var profileKey = _selectedProfile.Key;
            
            var chapterKey = new ChapterKey(summary.ChapterId, profileKey);
            var chapter = await StudyingService.SelectChapter(chapterKey);
            
            var profile = await StudyingService.GetProfile(profileKey);

            var selection = profile.CurrentChapter;

            if (selection == null)
                throw new InvalidOperationException($"no chapter selected for {profileKey}");

            _selectedProfile = profile;
            _selectedChapter = chapter;
            _selectedPage = chapter.Pages[selection.PageIndex];
        }
        catch (Exception error)
        {
            _error = error.Message 
                     + Environment.NewLine + error.StackTrace
                     + Environment.NewLine + error.InnerException?.Message 
                     + Environment.NewLine + error.InnerException?.StackTrace;
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task UpdateChapterAndPageAsync()
    {
        if (_selectedProfile is null)
            return;
        
        if (_selectedProfile.CurrentChapter is null)
        {
            _selectedChapter = null;
            _selectedPage = null;
        }
        else if (_selectedChapter is not null)
        {
            _selectedPage = _selectedChapter.Pages[_selectedProfile.CurrentChapter.PageIndex];
        }
        
        await InvokeAsync(StateHasChanged);
    }
}

