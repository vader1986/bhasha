using Bhasha.Domain.Interfaces;
using Microsoft.JSInterop;

namespace Bhasha.Infrastructure.BlazorSpeechSynthesis;

public class BlazorSpeaker(ISpeechSynthesisService speechSynthesisService) : ISpeaker
{
    private readonly List<SpeechSynthesisVoice> _voices = new();
    
    public async ValueTask<bool> IsLanguageSupported(string language)
    {
        if (_voices.Count == 0)
        {
            _voices.AddRange(await speechSynthesisService.GetVoicesAsync());
        }
        
        return _voices.Any(x => x.Lang.StartsWith(language));
    }

    public async Task SpeakAsync(string text, string language)
    {
        var voice = _voices.FirstOrDefault(x => x.Lang.StartsWith(language)) 
                    ?? _voices.FirstOrDefault(x => x.Default);
        
        if (voice is null)
            return;

        await speechSynthesisService.SpeakAsync(new SpeechSynthesisUtterance
        {
            Text = text
        });
    }
}