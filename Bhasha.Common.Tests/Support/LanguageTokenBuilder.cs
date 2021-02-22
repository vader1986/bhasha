namespace Bhasha.Common.Tests.Support
{
    public class LanguageTokenBuilder
    {
        private Language _language = Languages.English;
        private string _native = "cat";
        private string _spoken = "cat";
        private ResourceId _audioId;

        public static LanguageTokenBuilder Default => new LanguageTokenBuilder();
        public static LanguageToken Create() => Default.Build();

        public LanguageTokenBuilder WithLanguage(Language language)
        {
            _language = language;
            return this;
        }

        public LanguageTokenBuilder WithNative(string native)
        {
            _native = native;
            return this;
        }

        public LanguageTokenBuilder WithSpoken(string spoken)
        {
            _spoken = spoken;
            return this;
        }

        public LanguageTokenBuilder WithAudioId(ResourceId audioId)
        {
            _audioId = audioId;
            return this;
        }

        public LanguageToken Build()
        {
            return new LanguageToken(_language, _native, _spoken, _audioId);
        }
    }
}
