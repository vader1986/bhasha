using Bhasha.Common.Database;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class DbTranslationBuilder
    {
        private string _native = Rnd.Create.NextString();
        private string _spoken = Rnd.Create.NextString();
        private ResourceId _audioId;

        public static DbTranslationBuilder Default => new();

        public DbTranslationBuilder WithNative(string native)
        {
            _native = native;
            return this;
        }

        public DbTranslationBuilder WithSpoken(string spoken)
        {
            _spoken = spoken;
            return this;
        }

        public DbTranslationBuilder WithAudioId(ResourceId audioId)
        {
            _audioId = audioId;
            return this;
        }

        public DbTranslation Build()
        {
            return new DbTranslation
            {
                Native = _native,
                Spoken = _spoken,
                AudioId = _audioId
            };
        }
    }
}
