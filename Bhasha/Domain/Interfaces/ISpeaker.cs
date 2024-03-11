namespace Bhasha.Domain.Interfaces;

public interface ISpeaker
{
    bool IsLanguageSupported(string language);
    Task SpeakAsync(string text, string language);
}