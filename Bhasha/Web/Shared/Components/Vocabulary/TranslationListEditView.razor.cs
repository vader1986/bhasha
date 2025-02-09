using Bhasha.Domain;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class TranslationListEditView : ComponentBase
{
    [Inject] public required IDialogService DialogService { get; set; }
    [Parameter] public required List<TranslationEditViewModel> Values { get; set; }
    [Parameter] public EventCallback<List<TranslationEditViewModel>> ValuesChanged { get; set; }
    
    private bool DisableAdd => MissingLanguages.Count == 0;

    private List<Language> MissingLanguages { get; set; } = [];

    protected override void OnParametersSet()
    {
        MissingLanguages = Language.Supported.Values
            .Except(Values
                .Select(x => (Language)x.Language))
            .ToList();
        
        base.OnParametersSet();
    }

    private async Task OnAddAsync()
    {
        var dialog = await DialogService.ShowAsync<TranslationCreateDialog>(
            title: "Add Translation",
            new DialogParameters
            {
                ["MissingLanguages"] = MissingLanguages.ToList()
            });

        var result = await dialog.Result;
    
        if (result is null || result.Canceled)
            return;
    
        if (result.Data is TranslationEditViewModel translation)
        {
            Values.Add(translation);
            
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task OnChangeAsync(TranslationEditViewModel value)
    {
        var newStatus = HasNotChanged() 
            ? TranslationViewModelStatus.Initial 
            : TranslationViewModelStatus.Changed;

        if (newStatus != value.Status)
        {
            value.Status = newStatus;
            
            await ValuesChanged.InvokeAsync();
        }
        
        return;

        bool HasNotChanged() => value.Origin is not null &&
                             value.Origin.Text == value.Text &&
                             value.Origin.Spoken == value.Spoken;
    }
    
    private async Task OnDeleteAsync(TranslationEditViewModel value)
    {
        if (value.Status != TranslationViewModelStatus.Deleted)
        {
            value.Status = TranslationViewModelStatus.Deleted;

            MissingLanguages.Remove(value.Language);
            
            await ValuesChanged.InvokeAsync();
        }
    }
}

