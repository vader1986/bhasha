using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Domain.Pages;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class MultipleChoicePage : ComponentBase
{
    [Inject] public required ISpeaker Speaker { get; set; }
    [Inject] public required ILogger<MultipleChoicePage> Logger { get; set; }
    [Parameter] public required DisplayedPage<MultipleChoice> Data { get; set; }
    [Parameter] public required Func<Translation, Task> Submit { get; set; }

    private bool DisableAudio => _selectedChoice == null;
    private bool DisableSubmit => _selectedChoice == null;
    
    private MultipleChoice? _arguments;
    private Translation? _selectedChoice;
    private bool _playAudio;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _selectedChoice = null;
        _arguments = Data.Arguments;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (!_playAudio)
            return;
        
        if (_selectedChoice == null)
            return;

        var translation = _selectedChoice;
        var languageSupported = await Speaker.IsLanguageSupported(translation.Language);

        try
        {
            if (string.IsNullOrWhiteSpace(translation.Spoken) || languageSupported)
            {
                await Speaker.SpeakAsync(translation.Text, translation.Language);
            }
            else
            {
                await Speaker.SpeakAsync(translation.Spoken, translation.Language);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to play audio for {Translation}", translation.Text);
        }
        finally
        {
            _playAudio = false;
        }
    }

    private void OnPlayAudioClicked()
    {
        _playAudio = true;
        StateHasChanged();
    }

    private void OnSubmit()
    {
        if (_selectedChoice == null)
            return;

        Submit(_selectedChoice);
    }
}

