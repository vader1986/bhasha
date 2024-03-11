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
    [Parameter] public required DisplayedPage<MultipleChoice> Data { get; set; }
    [Parameter] public required Func<Translation, Task> Submit { get; set; }

    private bool DisableAudio => _selectedChoice == null;
    private bool DisableSubmit => _selectedChoice == null;
    
    private MultipleChoice? _arguments;
    private MudChip? _selectedChoice;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _selectedChoice = null;
        _arguments = Data.Arguments;
    }

    private async Task OnPlayAudioClicked()
    {
        if (_selectedChoice == null)
            return;

        var translation = (Translation)_selectedChoice.Value;

        if (string.IsNullOrWhiteSpace(translation.Spoken) || Speaker.IsLanguageSupported(translation.Language))
        {
            await Speaker.Speak(translation.Text, translation.Language);
        }
        else
        {
            await Speaker.Speak(translation.Spoken, translation.Language);
        }
    }

    private void OnSubmit()
    {
        if (_selectedChoice == null)
            return;

        Submit((Translation)_selectedChoice.Value);
    }
}

