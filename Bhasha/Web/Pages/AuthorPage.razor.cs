using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Services;
using Bhasha.Web.Shared.Components;
using Bhasha.Web.Shared.Components.Vocabulary;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Pages;

public partial class AuthorPage : UserPage
{
    private enum DisplayMode
    {
        DisplayOptions,
        DisplayChapterList,
        DisplayStudyCards
    }
    
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required IChapterRepository ChapterRepository { get; set; }
    [Inject] public required IAuthoringService AuthoringService { get; set; }

    private DisplayMode _displayMode = DisplayMode.DisplayOptions;

    private string? _error;

    private void SwitchToDisplayStudyCards() 
        => _displayMode = DisplayMode.DisplayStudyCards;
    
    private async Task OnImportWordsSelectedAsync()
    {
        await DialogService.ShowAsync<TextImportDialog>();
    }
    
    private async Task OnExpressionsSelectedAsync()
    {
        await DialogService.ShowAsync<ExpressionEditDialog>();
    }
    
    private async Task OnEditChapterClicked()
    {
        _displayMode = DisplayMode.DisplayChapterList;
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task OnCreateChapterClicked()
    {
        try
        {
            var parameters = new DialogParameters
            {
                { "UserId", UserId }
            };
            var dialog = await DialogService.ShowAsync<EditChapter>("Chapter", parameters);
            var result = await dialog.Result;

            if (result?.Data is Chapter chapter)
            {
                await AuthoringService.AddOrUpdateChapter(chapter);
                _displayMode = DisplayMode.DisplayOptions;
            }
            else
            {
                _displayMode = DisplayMode.DisplayChapterList;
            }
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
        
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task OnChapterSelected(Summary summary)
    {
        var chapterToEdit = await ChapterRepository
            .FindById(summary.ChapterId, CancellationToken.None);

        try
        {
            var parameters = new DialogParameters
            {
                { "UserId", UserId },
                { "Chapter", chapterToEdit }
            };
            var dialog = await DialogService.ShowAsync<EditChapter>("Chapter", parameters);
            var result = await dialog.Result;

            if (result?.Data is Chapter chapter)
            {
                await AuthoringService.AddOrUpdateChapter(chapter);
            }
        }
        catch (Exception e)
        {
            _error = e.Message;
        }

        _displayMode = DisplayMode.DisplayOptions;
        await InvokeAsync(StateHasChanged);
    }
}