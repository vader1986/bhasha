using Bhasha.Domain.Interfaces;
using Toolbelt.Blazor.SpeechSynthesis;

namespace Bhasha.Infrastructure.Toolbelt;

public class ToolbeltSpeaker(SpeechSynthesis speechSynthesis) : ISpeaker
{
    public bool IsLanguageSupported(string language)
    {
        return language switch
        {
            "en" => true,
            _ => false
        };
    }

    public async Task SpeakAsync(string text, string language)
    {
        await speechSynthesis.SpeakAsync(new SpeechSynthesisUtterance
        {
            Text = text,
            Lang = language
        });
    }
}