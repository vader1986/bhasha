using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class AudioButton : ComponentBase
{
    [Inject] public required ITranslationRepository TranslationRepository { get; set; }

    [Parameter] public required string Language { get; set; }
    [Parameter] public required string? Text { get; set; }
    
    private bool Disabled => Text == null;
    
    private Translation? _translation;
    private bool _isPlaying;
    private string _audioId = Guid.NewGuid().ToString();
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        
        if (!_isPlaying)
            return;
        
        if (Text is null)
            return;
        
        _translation = await TranslationRepository
            .Find(text: Text, language: Language);
        
        _isPlaying = false;
    }

    private async Task PlayAudioAsync()
    {
        _isPlaying = true;
        _audioId = Guid.NewGuid().ToString();
        
        await InvokeAsync(StateHasChanged);
    }
}

