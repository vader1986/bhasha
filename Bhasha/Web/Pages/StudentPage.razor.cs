using Bhasha.Domain;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Profile = Bhasha.Domain.Profile;

namespace Bhasha.Web.Pages;

public partial class StudentPage : UserPage
{
    [Inject] public required IStudyingService StudyingService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    
    private Profile? _selectedProfile;
    private DisplayedChapter? _selectedChapter;
    private DisplayedPage? _selectedPage;
    private string? _error;

    private bool DisplayProfileSelection => _selectedProfile == null;
    private bool DisplayChapterSelection => !DisplayProfileSelection && _selectedChapter == null;
    private bool DisplayPage => _selectedProfile != null && _selectedChapter != null && _selectedPage != null;

    private readonly IList<Profile> _profiles = new List<Profile>();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            if (UserId == null)
                throw new InvalidOperationException("user not logged in");
            
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
            StateHasChanged();
        }
    }

    internal void OnSelectProfile(Profile profile)
    {
        _selectedProfile = profile;

        StateHasChanged();
    }

    internal async Task OnSubmit(Translation translation)
    {
        try
        {
            var key = _selectedProfile?.Key;

            if (key == null)
                throw new InvalidOperationException("no profile selected");

            if (_selectedPage == null)
                throw new InvalidOperationException("no page selected");

            var expression = _selectedPage.Word.Expression;
            var userInput = new ValidationInput(key, expression.Id, translation);

            var validation = await StudyingService.Submit(userInput);

            switch (validation.Result)
            {
                case ValidationResult.Correct:
                    Snackbar.Add("Correct!", Severity.Success);
                    break;
                
                case ValidationResult.PartiallyCorrect:
                    Snackbar.Add("Almost correct!", Severity.Success);
                    break;
                
                case ValidationResult.Wrong:
                    Snackbar.Add("Wrong!", Severity.Error);
                    break;
            }
            
            OnProfileUpdated(await StudyingService.GetProfile(key));
        }
        catch (Exception error)
        {
            _error = error.Message;
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    internal async Task OnChapterSelected(DisplayedSummary summary)
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

    private void OnProfileUpdated(Profile profile)
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
}

