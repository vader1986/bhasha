namespace Bhasha;

public sealed class ResourcesSettings
{
    public const string SectionName = "Resources";

    public string BaseUrl { get; set; } = ".";
    
    public string GetImageUrl(string resourceId)
    {
        return $"{BaseUrl}/images/{resourceId}";
    }
}