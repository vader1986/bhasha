using System.Speech.Synthesis;
using Bhasha.Domain;
using Bhasha.Domain.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhasha.Web.Shared.Components;

public partial class MultipleChoicePage : ComponentBase
{
    [Parameter] public required DisplayedPage<MultipleChoice> Data { get; set; }
    [Parameter] public required Func<Translation, Task> Submit { get; set; }

    private bool DisableAudio => _selectedChoice == null ||
                                 _selectedChoice.Value is Translation translation && string.IsNullOrWhiteSpace(translation.Spoken) ||
                                 Environment.OSVersion.Platform == PlatformID.MacOSX;
    private bool DisableSubmit => _selectedChoice == null;
    
    private MultipleChoice? _arguments;
    private MudChip? _selectedChoice;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _selectedChoice = null;
        _arguments = Data.Arguments;
    }

    private void OnPlayAudioClicked()
    {
        if (_selectedChoice == null)
            return;

        var translation = (Translation)_selectedChoice.Value;
        
        if (string.IsNullOrWhiteSpace(translation.Spoken))
            return;

        if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            return;
        
        using var synthesizer = new SpeechSynthesizer();
        synthesizer.SetOutputToDefaultAudioDevice();
        synthesizer.SpeakAsync(translation.Spoken);
    }

    private void OnSubmit()
    {
        if (_selectedChoice == null)
            return;

        Submit((Translation)_selectedChoice.Value);
    }
}

