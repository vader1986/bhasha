using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bhasha.Web.Shared.Components;

public partial class AudioButton : ComponentBase
{
    [Inject] public required ITranslationProvider TranslationProvider { get; set; }

    [Parameter] public required string Language { get; set; }
    [Parameter] public required string? Text { get; set; }
    [Parameter] public required int ExpressionId { get; set; }
    
    private bool Disabled => Text == null || _isPlaying;
    
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

        _translation = await TranslationProvider
            .Find(ExpressionId, Language);
        
        _isPlaying = false;
    }

    private async Task PlayAudioAsync()
    {
        _isPlaying = true;
        _audioId = Guid.NewGuid().ToString();
        
        await InvokeAsync(StateHasChanged);
    }
}

