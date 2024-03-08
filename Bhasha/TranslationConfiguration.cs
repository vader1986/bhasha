using Bhasha.Infrastructure.AzureSpeechApi;
using Bhasha.Infrastructure.AzureTranslatorApi;

namespace Bhasha;

public class TranslationConfiguration
{ 
    public AzureTranslatorApiSettings? AzureTranslatorApi { get; init; }
    public AzureSpeechApiSettings? AzureSpeechApi { get; init; }
}