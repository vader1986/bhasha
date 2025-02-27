using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class AudioButton : ComponentBase
{
    [Inject] public required ISpeaker Speaker { get; set; }
    [Inject] public required ILogger<MultipleChoicePage> Logger { get; set; }

    [Parameter] public Translation? Translation { get; set; }

    private bool Disabled => Translation == null;

    private bool _playAudio;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (!_playAudio)
            return;
        
        if (Translation is null)
            return;
        
        var languageSupported = await Speaker.IsLanguageSupported(Translation.Language);

        try
        {
            if (string.IsNullOrWhiteSpace(Translation.Spoken) || languageSupported)
            {
                await Speaker.SpeakAsync(Translation.Text, Translation.Language);
            }
            else
            {
                await Speaker.SpeakAsync(Translation.Spoken, Translation.Language);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to play audio for {Translation}", Translation.Text);
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

