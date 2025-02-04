using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class TranslationListEditView : ComponentBase
{
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ITranslationRepository TranslationRepository { get; set; }
    
    [Parameter] public required Expression Expression { get; set; }
    
    private List<Language> AvailableLanguages => Language.Supported.Values
        .Except(_translations.Select(x => (Language)x.Origin.Language))
        .ToList();
    
    private bool DisableAddTranslation => AvailableLanguages.Count == 0;
    
    private List<TranslationEditViewModel> _translations = [];
    private string? _error;

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            var translations = await TranslationRepository.Find(Expression.Id);
        
            _translations = translations
                .Select(TranslationEditViewModel.Create)
                .ToList();
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
        
        await base.OnParametersSetAsync();
    }

    private async Task OnAddAsync()
    {
        var dialog = await DialogService.ShowAsync<TranslationCreateDialog>(
            title: "Add Translation",
            new DialogParameters
            {
                ["Expression"] = Expression,
                ["UsedLanguages"] = _translations.Select(x => x.Origin.Language).ToList()
            });

        var result = await dialog.Result;
        
        if (result is null || result.Canceled)
            return;
        
        if (result.Data is TranslationEditViewModel translation)
        {
            _translations.Add(translation);
            
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task OnDeleteAsync(TranslationEditViewModel translation)
    {
        try
        {
            if (translation.Origin.Id > 0)
            {
                await TranslationRepository.Delete(translation.Origin.Id);
            }

            _translations.Remove(translation);

            await InvokeAsync(StateHasChanged);
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
    }
}

