using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Pages.Author;

public partial class AddChapter : UserPage
{
    private readonly AddChapterState _state = new AddChapterState();

    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IRepository<Chapter> ChapterRepository { get; set; } = default!;
    [Inject] public ITranslationManager TranslationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _state.ChapterRepository = ChapterRepository;
        _state.TranslationManager = TranslationManager;
        _state.UserId = UserId;
    }

    internal void OnSelectedNative(MudChip chip)
    {
        _state.NativeLanguage = (chip?.Value as Language)?.ToString();
    }

    internal void OnSelectedTarget(MudChip chip)
    {
        _state.TargetLanguage = (chip?.Value as Language)?.ToString();
    }

    internal async Task OpenCreatePage()
    {
        var result = await DialogService.Show<AddPage>("Create Page").Result;

        if (!result.Cancelled)
        {
            await _state.SubmitPageState((AddPageState)result.Data);
        }
    }
}