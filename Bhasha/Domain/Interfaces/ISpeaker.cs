namespace Bhasha.Domain.Interfaces;

public interface ISpeaker
{
    bool IsLanguageSupported(string language);
    Task Speak(string text, string language);
}