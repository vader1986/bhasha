using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class Audio : ComponentBase
{
    [Inject] public required ResourcesSettings Resources { get; set; }
    [Inject] public required ISpeaker Speaker { get; set; }
    [Inject] public required ILogger<AudioButton> Logger { get; set; }
    
    [Parameter] public required Translation Translation { get; set; }

    private string _id = Guid.NewGuid().ToString();
    private bool _playAudio;
    private string? _audioFileName;

    protected override void OnParametersSet()
    {
        _id = Guid.NewGuid().ToString();
        _playAudio = false;
        
        base.OnParametersSet();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (_playAudio)
            return;

        try
        {
            if (string.IsNullOrWhiteSpace(Translation.AudioId))
            {
                await Speaker
                    .SpeakAsync(
                        text: Translation.Text, 
                        language: Translation.Language,
                        transliteration: Translation.Spoken);
            }
            else
            {
                _audioFileName = Resources
                    .GetAudioFile(Translation.AudioId);
            }

            _playAudio = true;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to play audio for {Text} in {Language}", Translation.Text, Translation.Language);
        }
    }
}

