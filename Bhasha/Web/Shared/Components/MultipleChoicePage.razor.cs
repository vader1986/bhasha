using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Domain.Pages;
using Bhasha.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Toolbelt.Blazor.SpeechSynthesis;

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
    private MudChip? _selectedChoice;
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

        var translation = (Translation)_selectedChoice.Value;

        try
        {
            if (string.IsNullOrWhiteSpace(translation.Spoken) || Speaker.IsLanguageSupported(translation.Language))
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

        Submit((Translation)_selectedChoice.Value);
    }
}

