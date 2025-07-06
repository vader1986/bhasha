using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Bhasha.Web.Shared.Components.Vocabulary;

public partial class TranslationCreateDialog : ComponentBase
{
    [Inject] public required ITranslator Translator { get; set; }
    [Inject] public required IResourcesManager ResourcesManager { get; set; }

    [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }
    [Parameter] public List<Language> MissingLanguages { get; set; } = [];
    [Parameter] public Language? Language { get; set; }
    [Parameter] public string? ReferenceTranslation { get; set; }

    private Language? _language;
    private string? _text;
    private string? _spoken;
    private string? _audioId;
    private string? _error;
    
    private bool DisableAdd => !string.IsNullOrWhiteSpace(_error) || string.IsNullOrWhiteSpace(_text) || _language is null;
    
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
    
    private async Task OnTranslationChanged()
    {
        ValidateInputs();
        await InvokeAsync(StateHasChanged);
    }

    private void ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(_text))
        {
            _error = "Translation is required";
            return;
        }

        if (_language is null)
        {
            _error = "Language is required";
            return;
        }
        
        if (_text.Length > 100)
        {
            _error = "Translation is too long";
            return;
        }
        
        _error = null;
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
            result.AudioId = _audioId;
            
            MudDialog.Close(DialogResult.Ok(result));
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
    }
    
    private async Task OnResourceChanged(IBrowserFile? audioFile)
    {
        try
        {
            if (audioFile is null)
                return;

            await ResourcesManager.UploadAudio(audioFile.Name, audioFile.OpenReadStream());

            _audioId = audioFile.Name;
        }
        catch (Exception e)
        {
            _error = e.Message;
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    private void OnCancel()
    {
        MudDialog.Cancel();
    }
}

