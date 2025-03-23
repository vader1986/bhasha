using Bhasha.Domain.Interfaces;
using Microsoft.CognitiveServices.Speech;

namespace Bhasha.Infrastructure.AzureSpeechApi;

public class AzureSpeaker(ILogger<AzureSpeaker> logger, AzureSpeechApiSettings settings) : ISpeaker
{
    private static string ConvertLanguageForAzure(string language)
    {
        return language switch
        {
            "bn" => "bn-IN",
            _ => language
        };
    }
    
    public async Task SpeakAsync(string text, string language, string? transliteration)
    {
        try
        {
            var config = SpeechConfig.FromSubscription(settings.Key, settings.Region);
            config.SpeechSynthesisLanguage = ConvertLanguageForAzure(language);
            using var synthesizer = new SpeechSynthesizer(config);
            var result = await synthesizer.SpeakTextAsync(text);
            if (result.Reason == ResultReason.Canceled)
            {
                var error = SpeechSynthesisCancellationDetails.FromResult(result);
                throw new InvalidOperationException(error.ErrorDetails);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "failed to convert text to speech: {Text} [{Language}]", text, language);
        }
    }
}