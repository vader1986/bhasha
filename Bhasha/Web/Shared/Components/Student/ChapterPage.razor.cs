using Bhasha.Domain;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Student;

public partial class ChapterPage : ComponentBase
{
    [Inject] public required IStudyingService StudyingService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    
    [Parameter] public required DisplayedChapter Chapter { get; set; }
    [Parameter] public required DisplayedPage Page { get; set; }
    [Parameter] public required Profile Value { get; set; }
    [Parameter] public EventCallback<Profile> ValueChanged { get; set; }

    private string _title = string.Empty;
    private string _description = string.Empty;
    private string _word = string.Empty;
    
    private int _progress;
    private int _index;
    private string? _userInput;
    private ChapterPageBarViewModel? _chapterPageBarViewModel;
    private ChapterPageViewModel? _viewModel;
    private PageType _pageType = PageType.MultipleChoice;
    
    protected override void OnParametersSet()
    {
        _viewModel = new ChapterPageViewModel(Value.Key, Chapter, Page);

        var suggestedPageType = (PageType)(_index % Enum.GetValues<PageType>().Length);
        
        _pageType = suggestedPageType switch
        {
            PageType.MultipleChoice 
                => PageType.MultipleChoice,
            PageType.Cloze when Page.Word.Text.Split(" ").Length >= 3
                => PageType.Cloze,
            PageType.ChooseNative
                => PageType.ChooseNative,
            _ 
                => PageType.MultipleChoice
        };
        
        _title = _viewModel.Chapter.Name;
        _description = _viewModel.Chapter.Description;
        _word = _viewModel.Page.Word.Text;
        
        _chapterPageBarViewModel = new ChapterPageBarViewModel(
            ProfileKey: Value.Key,
            ExpressionId: _viewModel.Page.Word.Expression.Id,
            UserInput: _userInput);
        
        _index = Value.CurrentChapter?.PageIndex ?? 0;

        UpdateProgress();
        
        base.OnParametersSet();
    }

    private void UpdateProgress()
    {
        if (_viewModel is null)
            return;
        
        var totalPages = _viewModel.Chapter.Pages.Length * 3;
            
        var correctAnswersPerPage = Value.CurrentChapter?.CorrectAnswers ?? [];
        var correctAnswers = correctAnswersPerPage.Sum(x => Math.Min(3, (int)x));
            
        _progress = (int)Math.Round(100 * (double)correctAnswers / totalPages);
    }

    private async Task OnValueChanged()
    {
        if (_chapterPageBarViewModel is not null)
        {
            _chapterPageBarViewModel = _chapterPageBarViewModel with
            {
                UserInput = _userInput
            };
        }
        
        await InvokeAsync(StateHasChanged);
    }

    private async Task UpdateProfileAsync()
    {
        var profile = await StudyingService.GetProfile(Value.Key);

        if (Value != profile)
        {
            _userInput = null;

            UpdateProgress();
            
            await ValueChanged.InvokeAsync(profile);
            
            await InvokeAsync(StateHasChanged);
        }
    }

    private void DisplayError(Exception error)
    {
        Snackbar.Add(error.Message, Severity.Error);
    }
    
    private enum PageType
    {
        MultipleChoice = 0,
        Cloze = 1,
        ChooseNative = 2
    }
}

