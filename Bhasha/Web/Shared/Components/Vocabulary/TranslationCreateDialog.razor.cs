using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class TranslationCreateDialog : ComponentBase
{
    [Inject] public required ITranslator Translator { get; set; }

    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }
    [Parameter] public List<Language> MissingLanguages { get; set; } = [];
    [Parameter] public Language? Language { get; set; }
    [Parameter] public string? ReferenceTranslation { get; set; }

    private Language? _language;
    private string? _text;
    private string? _spoken;
    private string? _error;
    
    private bool DisableAdd => !string.IsNullOrWhiteSpace(_error) || string.IsNullOrWhiteSpace(_text) || _language is null || !MissingLanguages.Contains(_language);

    protected override async Task OnParametersSetAsync()
    {
        _language = Language ?? MissingLanguages.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(ReferenceTranslation) &&
            _language is not null)
        {
            var (translation, spoken) = await Translator.Translate(ReferenceTranslation, _language);

            _text = translation;
            _spoken = spoken;
        }
        
        await base.OnParametersSetAsync();
    }

    private void OnLanguageChanged(Language? language)
    {
        _language = language;
        
        StateHasChanged();
    }
    
    private void OnTextChanged(string? text)
    {
        _text = text;
        
        StateHasChanged();
    }
    
    private void OnSpokenChanged(string? spoken)
    {
        _spoken = spoken;
        
        StateHasChanged();
    }
    
    private void OnAddAsync()
    {
        if (_text is null)
            return;
        
        if (_language is null)
            return;
        
        try
        {
            var result = TranslationEditViewModel.Create(_language);
            
            result.Text = _text;
            result.Spoken = _spoken;
            
            MudDialog.Close(DialogResult.Ok(result));
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
    }

    private void OnCancel()
    {
        MudDialog.Cancel();
    }
}

