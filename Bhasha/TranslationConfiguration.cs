using Bhasha.Infrastructure.AzureSpeechApi;
using Bhasha.Infrastructure.AzureTranslatorApi;

namespace Bhasha;

public class TranslationConfiguration
{
    public const string SectionName = "Translation";
    
    public AzureTranslatorApiSettings? AzureTranslatorApi { get; init; }
    public AzureSpeechApiSettings? AzureSpeechApi { get; init; }

    public TranslationProvider Provider { get; init; } = TranslationProvider.Toolbelt;
}

public enum TranslationProvider
{
    Azure,
    BlazorSpeechSynthesis,
    Toolbelt
}