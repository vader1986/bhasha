namespace Bhasha.Domain.Interfaces;

public interface ITranslator
{
    Task<(string Translation, string Spoken)> Translate(string text, string language);
}