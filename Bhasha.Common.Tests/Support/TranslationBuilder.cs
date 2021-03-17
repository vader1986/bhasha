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

        public static TranslationBuilder Default => new TranslationBuilder();

        public Translation Build()
        {
            return new Translation(_tokenId, _language, _native, _spoken, _audioId);
        }
    }
}
