using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class TranslatedWordBuilder
    {
        private Word _word = WordBuilder.Default.Build();
        private string _native = Rnd.Create.NextString();
        private string _spoken = Rnd.Create.NextString();
        private ResourceId _audioId;

        public static TranslatedWordBuilder Default => new();

        public TranslatedWordBuilder WithWord(Word word)
        {
            _word = word;
            return this;
        }

        public TranslatedWordBuilder WithNative(string native)
        {
            _native = native;
            return this;
        }

        public TranslatedWordBuilder WithSpoken(string spoken)
        {
            _spoken = spoken;
            return this;
        }

        public TranslatedWordBuilder WithAudioId(ResourceId audioId)
        {
            _audioId = audioId;
            return this;
        }

        public TranslatedWord Build()
        {
            return new TranslatedWord(_word, _native, _spoken, _audioId);
        }
    }
}
