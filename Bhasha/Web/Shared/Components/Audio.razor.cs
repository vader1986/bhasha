using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bhasha.Web.Shared.Components;

public partial class Audio : ComponentBase
{
    [Inject] public required ResourcesSettings Resources { get; set; }
    [Inject] public required IJSRuntime JsRuntime { get; set; }
    [Inject] public required ISpeaker Speaker { get; set; }
    [Inject] public required ILogger<AudioButton> Logger { get; set; }
    
    [Parameter] public required string Id { get; set; }
    [Parameter] public required Translation Translation { get; set; }

    private string _id = string.Empty;
    private string? _audioFileName;
    private bool _isPlaying;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_id == Id)
        {
            return;
        }
        
        _id = Id;
        
        
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
                
                _isPlaying = true;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to play audio for {Text} in {Language}", Translation.Text, Translation.Language);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (_isPlaying)
        {
            await JsRuntime
                .InvokeVoidAsync("PlaySound", _id);

            _isPlaying = false;
        }
    }
}

