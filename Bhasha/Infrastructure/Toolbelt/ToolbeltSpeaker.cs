using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Toolbelt.Blazor.SpeechSynthesis;

namespace Bhasha.Infrastructure.Toolbelt;

public class ToolbeltSpeaker(SpeechSynthesis speechSynthesis) : ISpeaker
{
    private static string DefaultLanguage => Language.Reference.ToString();
    
    private async Task<SpeechSynthesisVoice?> GetSuitableVoiceAsync(string language)
    {
        var voices = await speechSynthesis.GetVoicesAsync();

        var suitableVoices = voices
            .Where(x => x.Lang.StartsWith(language))
            .ToList();
        
        return suitableVoices.Count switch
        {
            0 => voices.FirstOrDefault(x => x.Default),
            1 => suitableVoices.First(),
            > 1 => suitableVoices[Random.Shared.Next(0, suitableVoices.Count - 1)],
            _ => null
        };
    }

    public async Task SpeakAsync(string text, string language, string? transliteration)
    {
        var voice = await GetSuitableVoiceAsync(language);

        if (voice is null && !string.IsNullOrWhiteSpace(transliteration))
        {
            text = transliteration;
            voice = await GetSuitableVoiceAsync(DefaultLanguage);
        }

        await speechSynthesis.SpeakAsync(new SpeechSynthesisUtterance
        {
            Text = text,
            Voice = voice
        });
    }
}