using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class TranslationCreateDialog : ComponentBase
{
    [Inject] public required ITranslationRepository TranslationRepository { get; set; }
    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }
    [Parameter] public required Expression Expression { get; set; }
    [Parameter] public IEnumerable<Language> UsedLanguages { get; set; } = [];

    private Language[] _languages = [];
    
    private Language? _language;
    private string? _text;
    private string? _error;
    
    private bool DisableAdd => !string.IsNullOrWhiteSpace(_error) || string.IsNullOrWhiteSpace(_text) || _language is null || UsedLanguages.Contains(_language);

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        
        _languages = Language
            .Supported.Values
            .Where(language => !UsedLanguages.Contains(language))
            .ToArray();
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
    
    private async Task OnAddAsync()
    {
        if (_text is null)
            return;
        
        if (_language is null)
            return;
        
        try
        {
            var translation = Translation.Create(Expression, _language, _text);
            
            translation = await TranslationRepository.AddOrUpdate(translation);

            var result = TranslationEditViewModel.Create(translation);
            
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

