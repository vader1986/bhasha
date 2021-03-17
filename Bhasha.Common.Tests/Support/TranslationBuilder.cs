using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class TranslationBuilder
    {
        private Guid _tokenId = Guid.NewGuid();
        private Language _language = Language.Supported.Values.Random();
        private string _native = Rnd.Create.NextString();
        private string _spoken = Rnd.Create.NextString();
        private ResourceId _audioId = Rnd.Create.NextString();

        public TranslationBuilder WithTokenId(Guid tokenId)
        {
            _tokenId = tokenId;
            return this;
        }

        public TranslationBuilder WithLanguage(Language language)
        {
            _language = language;
            return this;
        }

        public static TranslationBuilder Default => new();

        public Translation Build()
        {
            return new Translation(_tokenId, _language, _native, _spoken, _audioId);
        }
    }
}
