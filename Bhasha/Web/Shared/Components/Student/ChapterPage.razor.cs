﻿using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Student;

public partial class ChapterPage : ComponentBase
{
    [Inject] public required IStudyingService StudyingService { get; set; }
    [Inject] public required ITranslationProvider TranslationProvider { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    
    [Parameter] public required DisplayedChapter Chapter { get; set; }
    [Parameter] public required DisplayedPage Page { get; set; }
    [Parameter] public required Profile Value { get; set; }
    [Parameter] public EventCallback<Profile> ValueChanged { get; set; }

    private string _title = string.Empty;
    private string _description = string.Empty;
    
    private int _progress;
    private int _index;
    private string? _userInput;
    private ChapterPageBarViewModel? _chapterPageBarViewModel;
    private ChapterPageViewModel? _viewModel;
    private PageType _pageType = PageType.MultipleChoice;
    
    private Translation? _audioTranslation;
    private string _audioId = Guid.NewGuid().ToString();
    
    protected override void OnParametersSet()
    {
        _viewModel = new ChapterPageViewModel(Value.Key, Chapter, Page);

        var suggestedPageType = (PageType)(_index % Enum.GetValues<PageType>().Length);
        
        _pageType = suggestedPageType switch
        {
            PageType.MultipleChoice 
                => PageType.MultipleChoice,
            PageType.Cloze when HasMultipleWords(min: 3)
                => PageType.Cloze,
            PageType.ChooseImage when HasImages(min: 3)
                => PageType.ChooseImage,
            PageType.ChooseNative
                => PageType.ChooseNative,
            _ when HasMultipleWords(min: 3) 
                => PageType.Cloze,
            _ when HasImages(min: 3) 
                => PageType.ChooseImage,
            _
                => PageType.MultipleChoice
        };
        
        _title = _viewModel.Chapter.Name;
        _description = _viewModel.Chapter.Description;
        
        _chapterPageBarViewModel = new ChapterPageBarViewModel(
            ProfileKey: Value.Key,
            ExpressionId: _viewModel.Page.Word.Expression.Id,
            UserInput: _userInput);
        
        _index = Value.CurrentChapter?.PageIndex ?? 0;

        UpdateProgress();
        
        base.OnParametersSet();
        
        return;
        
        bool HasMultipleWords(int min)
            => Page.Word.Text.Split(" ").Length >= min;

        bool HasImages(int min)
            => Chapter.Pages.Count(x => x.Word.Expression.ResourceId is not null) >= min &&
               Page.Word.Expression.ResourceId is not null;
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

    private async Task PlayAudioAsync()
    {
        _audioTranslation = await TranslationProvider
            .Find(Page.Word.Expression.Id, Value.Key.Target);

        if (_audioTranslation is not null)
        {
            _audioId = Guid.NewGuid().ToString();

            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task UpdateProgressAsync()
    {
        await PlayAudioAsync();
        
        var profile = await StudyingService
            .GetProfile(Value.Key);
        
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
        ChooseNative = 1,
        Cloze = 2,
        ChooseImage = 3
    }
}

