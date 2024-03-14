using Bhasha.Domain.Interfaces;
using Toolbelt.Blazor.SpeechSynthesis;

namespace Bhasha.Infrastructure.Toolbelt;

public class ToolbeltSpeaker(SpeechSynthesis speechSynthesis) : ISpeaker
{
    public async ValueTask<bool> IsLanguageSupported(string language)
    {
        var voices = await speechSynthesis.GetVoicesAsync();
        return voices.Any(x => x.Lang.StartsWith(language));
    }

    public async Task SpeakAsync(string text, string language)
    {
        if (await IsLanguageSupported(language))
        {
            var voices = await speechSynthesis.GetVoicesAsync();
            var suitableVoices = voices.Where(x => x.Lang.StartsWith(language)).ToList();

            if (suitableVoices.Count > 1)
            {
                var random = new Random();
                var voice = suitableVoices[random.Next(0, suitableVoices.Count - 1)];
                
                await speechSynthesis.SpeakAsync(new SpeechSynthesisUtterance
                {
                    Text = text,
                    Voice = voice
                });
            }
            else
            {
                await speechSynthesis.SpeakAsync(new SpeechSynthesisUtterance
                {
                    Text = text,
                    Voice = suitableVoices.Single()
                });
            }
        }
        else
        {
            await speechSynthesis.SpeakAsync(new SpeechSynthesisUtterance
            {
                Text = text
            });
        }
    }
}