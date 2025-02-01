using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Services;
using Bhasha.Web.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Pages;

public partial class AuthorPage : UserPage
{
    private enum Mode
    {
        Overview,
        List
    }
    
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required IChapterRepository ChapterRepository { get; set; }
    [Inject] public required IAuthoringService AuthoringService { get; set; }

    private bool DisplayOverview => _mode == Mode.Overview;
    private bool DisplayChapterList => _mode == Mode.List;
    
    private Mode _mode = Mode.Overview;

    private string? _error;
    
    private async Task OnEditChapterClicked()
    {
        _mode = Mode.List;
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
                _mode = Mode.Overview;
            }
            else
            {
                _mode = Mode.List;
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

        _mode = Mode.Overview;
        await InvokeAsync(StateHasChanged);
    }
}