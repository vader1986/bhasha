using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bhasha.Web.Shared.Components;

public partial class AudioButton : ComponentBase
{
    [Inject] public required ResourcesSettings Resources { get; set; }
    [Inject] public required ISpeaker Speaker { get; set; }
    [Inject] public required ITranslationProvider TranslationProvider { get; set; }
    [Inject] public required IJSRuntime JsRuntime { get; set; }
    [Inject] public required ILogger<AudioButton> Logger { get; set; }

    [Parameter] public required string Language { get; set; }
    [Parameter] public required string? Text { get; set; }
    [Parameter] public required int ExpressionId { get; set; }

    private bool Disabled => Text == null;
    
    private bool _playAudio;
    private string? _audioFileName;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (!_playAudio)
            return;
        
        if (Text is null)
            return;

        try
        {
            var translation = await TranslationProvider
                .Find(ExpressionId, Language);

            if (string.IsNullOrWhiteSpace(translation?.AudioId))
            {
                await Speaker
                    .SpeakAsync(
                        text: Text, 
                        language: Language,
                        transliteration: translation?.Spoken);
            }
            else
            {
                _audioFileName = Resources.GetAudioFile(translation.AudioId);

                await JsRuntime
                    .InvokeVoidAsync("PlaySound", "sound");
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to play audio for {Text} in {Language}", Text, Language);
        }
        finally
        {
            _playAudio = false;
        }
    }

    private async Task PlayAudioAsync()
    {
        _playAudio = true;
        
        await InvokeAsync(StateHasChanged);
    }
}

