using System;
using Bhasha.Common.Database;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class DbTranslatedWordBuilder
    {
        private Guid _id = Guid.NewGuid();
        private CEFR _cefr = Rnd.Create.Choose(Enum.GetValues<CEFR>());
        private PartOfSpeech _partOfSpeech = Rnd.Create.Choose(Enum.GetValues<PartOfSpeech>());
        private DbTranslation _translation = DbTranslationBuilder.Default.Build();
        private ResourceId _pictureId;

        public static DbTranslatedWordBuilder Default => new();

        public DbTranslatedWordBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public DbTranslatedWordBuilder WithCefr(CEFR cefr)
        {
            _cefr = cefr;
            return this;
        }

        public DbTranslatedWordBuilder WithPartOfSpeech(PartOfSpeech partOfSpeech)
        {
            _partOfSpeech = partOfSpeech;
            return this;
        }

        public DbTranslatedWordBuilder WithTranslation(DbTranslation translation)
        {
            _translation = translation;
            return this;
        }

        public DbTranslatedWordBuilder WithPictureId(ResourceId pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public DbTranslatedWord Build()
        {
            return new DbTranslatedWord
            {
                Id = _id,
                Cefr = _cefr,
                PartOfSpeech = _partOfSpeech,
                Translation = _translation,
                PictureId = _pictureId
            };
        }
    }
}
