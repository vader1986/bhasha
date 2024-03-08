using Bhasha.Domain.Interfaces;

namespace Bhasha.Services;

public class NoSpeaker : ISpeaker
{
    public Task Speak(string text, string language)
    {
        return Task.CompletedTask;
    }
}