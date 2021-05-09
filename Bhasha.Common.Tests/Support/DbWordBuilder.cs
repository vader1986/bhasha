using System;
using System.Collections.Generic;
using Bhasha.Common.Database;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class DbWordBuilder
    {
        private Guid _id = Guid.NewGuid();
        private CEFR _cefr = Rnd.Create.Choose(Enum.GetValues<CEFR>());
        private PartOfSpeech _partOfSpeech = Rnd.Create.Choose(Enum.GetValues<PartOfSpeech>());
        private ResourceId _pictureId;
        private Dictionary<string, DbTranslation> _translations = new Dictionary<string, DbTranslation> {
            { Language.English, DbTranslationBuilder.Default.Build() },
            { Language.Bengali, DbTranslationBuilder.Default.Build() }
        };

        public static DbWordBuilder Default => new();

        public DbWordBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public DbWordBuilder WithCefr(CEFR cefr)
        {
            _cefr = cefr;
            return this;
        }

        public DbWordBuilder WithPartOfSpeech(PartOfSpeech partOfSpeech)
        {
            _partOfSpeech = partOfSpeech;
            return this;
        }

        public DbWordBuilder WithPictureId(ResourceId pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public DbWordBuilder WithTranslations(Dictionary<string, DbTranslation> translations)
        {
            _translations = translations;
            return this;
        }

        public DbWord Build()
        {
            return new DbWord {
                Id = _id,
                Cefr = _cefr,
                PartOfSpeech = _partOfSpeech,
                PictureId = _pictureId,
                Translations = _translations
            };
        }
    }
}
