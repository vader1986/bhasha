using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Bhasha.Domain.Interfaces;

namespace Bhasha.Infrastructure.AzureTranslatorApi;

public class AzureTranslatorApiClient : ITranslator
{
    private readonly ILogger<AzureTranslatorApiClient> _logger;
    private readonly AzureTranslatorApiSettings _settings;

    public AzureTranslatorApiClient(ILogger<AzureTranslatorApiClient> logger, AzureTranslatorApiSettings settings)
    {
        _logger = logger;
        _settings = settings;
    }
    
    public async Task<(string Translation, string Spoken)> Translate(string text, string language)
    {
        try
        {
            var route = $"/translate?api-version=3.0&from=en&to={language}&toScript=Latn";
            object[] body = [new { Text = text }];
            var requestBody = JsonSerializer.Serialize(body);

            using var client = new HttpClient();
            using var request = new HttpRequestMessage();
        
            // Build the request.
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(_settings.Endpoint + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", _settings.Key);
            // location required if you're using a multi-service or regional (not global) resource.
            request.Headers.Add("Ocp-Apim-Subscription-Region", _settings.Region);

            // Send the request and get response.
            var response = await client
                .SendAsync(request)
                .ConfigureAwait(false);
        
            // Read response as a string.
            var stream = await response.Content.ReadAsStreamAsync();
            var entries = await JsonSerializer.DeserializeAsync<JsonArray>(stream) ?? throw new InvalidOperationException("failed to parse response");
            var translations = entries[0]?["translations"]?.AsArray() ?? throw new InvalidOperationException($"failed to parse {entries}");
            var target = translations[0]?.AsObject() ?? throw new InvalidOperationException($"failed to parse {translations}");

            var translation = target["text"]?.GetValue<string>() ?? "";
            var spoken = target["transliteration"]?["text"]?.GetValue<string>() ?? "";
            
            return (translation, spoken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to fetch translation from Azure Translator API");
            return ("", "");
        }
    }
}