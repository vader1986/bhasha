namespace Bhasha.Domain.Interfaces;

public interface ISpeaker
{
    Task SpeakAsync(string text, string language, string? transliteration);
}