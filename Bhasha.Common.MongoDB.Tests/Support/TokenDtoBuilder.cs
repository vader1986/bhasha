using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class TokenDtoBuilder
    {
        private string _languageId = Languages.English;
        private string _native = "cat";
        private string _spoken = "cat";
        private string _audioId;

        public static TokenDtoBuilder Default => new TokenDtoBuilder();
        public static TokenDto Create() => Default.Build();

        public TokenDtoBuilder WithLanguageId(string languageId)
        {
            _languageId = languageId;
            return this;
        }

        public TokenDtoBuilder WithNative(string native)
        {
            _native = native;
            return this;
        }

        public TokenDtoBuilder WithSpoken(string spoken)
        {
            _spoken = spoken;
            return this;
        }

        public TokenDtoBuilder WithAudioId(string audioId)
        {
            _audioId = audioId;
            return this;
        }

        public TokenDto Build()
        {
            return new TokenDto {
                LanguageId = _languageId,
                Native = _native,
                Spoken = _spoken,
                AudioId = _audioId
            };
        }
    }
}
