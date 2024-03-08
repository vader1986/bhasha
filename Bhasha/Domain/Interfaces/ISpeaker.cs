namespace Bhasha.Domain.Interfaces;

public interface ISpeaker
{
    Task Speak(string text, string language);
}