namespace Bhasha.Domain.Interfaces;

public interface ISpeaker
{
    ValueTask<bool> IsLanguageSupported(string language);
    Task SpeakAsync(string text, string language);
}