namespace Bhasha.Infrastructure.AzureTranslatorApi;

public class AzureTranslatorApiSettings
{
    public required string Endpoint { get; init; }
    public required string Key { get; init; }
    public required string Region { get; init; }
}