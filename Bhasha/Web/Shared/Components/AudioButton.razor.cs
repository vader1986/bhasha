using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class AudioButton : ComponentBase
{
    [Inject] public required ISpeaker Speaker { get; set; }
    [Inject] public required ILogger<AudioButton> Logger { get; set; }
    [Inject] public required ITranslationProvider TranslationProvider { get; set; }

    [Parameter] public required string Language { get; set; }
    [Parameter] public required string? Text { get; set; }
    [Parameter] public required int ExpressionId { get; set; }

    private bool Disabled => Text == null;

    private bool _playAudio;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (!_playAudio)
            return;
        
        if (Text is null)
            return;
        
        var languageSupported = await Speaker.IsLanguageSupported(Language);

        try
        {
            if (languageSupported)
            {
                await Speaker.SpeakAsync(Text, Language);
            }
            else
            {
                var translation = await TranslationProvider.Find(expressionId: ExpressionId, language: Language);
                if (translation?.Spoken is not null)
                {
                    await Speaker.SpeakAsync(
                        text: translation.Spoken,
                        language: Domain.Language.Reference);
                }
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

