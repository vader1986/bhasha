using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class WordBuilder
    {
        private Guid _id = Guid.NewGuid();
        private PartOfSpeech _partOfSpeech = Rnd.Create.Choose(Enum.GetValues<PartOfSpeech>());
        private CEFR _cefr = Rnd.Create.Choose(Enum.GetValues<CEFR>());
        private ResourceId _pictureId;

        public static WordBuilder Default => new();

        public WordBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public WordBuilder WithPartOfSpeech(PartOfSpeech partOfSpeech)
        {
            _partOfSpeech = partOfSpeech;
            return this;
        }

        public WordBuilder WithCefr(CEFR cefr)
        {
            _cefr = cefr;
            return this;
        }

        public WordBuilder WithPictureId(ResourceId pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public Word Build()
        {
            return new Word(_id, _partOfSpeech, _cefr, _pictureId);
        }
    }
}
