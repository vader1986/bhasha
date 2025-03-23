namespace Bhasha;

public sealed class ResourcesSettings
{
    public const string SectionName = "Resources";

    public string BaseUrl { get; set; } = ".";
    
    public string GetImageFile(string resourceId)
    {
        return $"{BaseUrl}/images/{resourceId}";
    }
    
    public string GetAudioFile(string resourceId)
    {
        return $"{BaseUrl}/audio/{resourceId}";
    }
}