using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class Audio : ComponentBase
{
    [Inject] public required ResourcesSettings Resources { get; set; }
    [Inject] public required ISpeaker Speaker { get; set; }
    [Inject] public required ILogger<AudioButton> Logger { get; set; }
    
    [Parameter] public required string Id { get; set; }
    [Parameter] public required Translation Translation { get; set; }

    private string _id = string.Empty;
    private bool _startPlaying;
    private string? _audioFileName;

    private string? _debug;
    
    protected override void OnParametersSet()
    {
        if (_id != Id)
        {
            _startPlaying = true;
            _id = Id;
        }
        
        base.OnParametersSet();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (!_startPlaying)
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
                
                _debug = "Speaker: " + Translation.Text + " " + Translation.Language + " " + Translation.Spoken;
            }
            else
            {
                _audioFileName = Resources
                    .GetAudioFile(Translation.AudioId);

                _debug = _audioFileName;
            }
        }
        catch (Exception e)
        {
            _debug = e.Message + " " + e.StackTrace;
            Logger.LogError(e, "Failed to play audio for {Text} in {Language}", Translation.Text, Translation.Language);
        }
        finally
        {
            _startPlaying = false;
        }
    }
}

