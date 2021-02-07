#nullable enable
namespace Bhasha.Common
{
    public class LanguageToken
    {
        public Language Language { get; }
        public string Native { get; }
        public string Spoken { get; }
        public ResourceId? AudioId { get; }

        public LanguageToken(Language language, string native, string spoken, ResourceId? audioId = default)
        {
            Language = language;
            Native = native;
            Spoken = spoken;
            AudioId = audioId;
        }
    }
}
