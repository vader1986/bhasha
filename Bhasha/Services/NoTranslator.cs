using Bhasha.Domain.Interfaces;

namespace Bhasha.Services;

public class NoTranslator : ITranslator
{
    public Task<(string Translation, string Spoken)> Translate(string text, string language)
    {
        return Task.FromResult(("", ""));
    }
}