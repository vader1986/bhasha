using System;
using System.Linq;
using Bhasha.Common.Database;

namespace Bhasha.Common.Tests.Support
{
    public class DbTranslatedChapterBuilder
    {
        private Guid _id = Guid.NewGuid();
        private int _level = Rnd.Create.Next(1, 10);
        private DbTranslatedExpression _name = DbTranslatedExpressionBuilder.Default.Build();
        private DbTranslatedExpression _description = DbTranslatedExpressionBuilder.Default.Build();
        private DbProfile _languages = DbProfileBuilder.Default.Build();
        private DbTranslatedPage[] _pages = Enumerable.Range(2, 5).Select(_ => DbTranslatedPageBuilder.Default.Build()).ToArray();
        private ResourceId _pictureId;

        public static DbTranslatedChapterBuilder Default => new();

        public DbTranslatedChapterBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public DbTranslatedChapterBuilder WithLevel(int level)
        {
            _level = level;
            return this;
        }

        public DbTranslatedChapterBuilder WithName(DbTranslatedExpression name)
        {
            _name = name;
            return this;
        }

        public DbTranslatedChapterBuilder WithDescription(DbTranslatedExpression description)
        {
            _description = description;
            return this;
        }

        public DbTranslatedChapterBuilder WithLanguages(DbProfile languages)
        {
            _languages = languages;
            return this;
        }

        public DbTranslatedChapterBuilder WithPages(DbTranslatedPage[] pages)
        {
            _pages = pages;
            return this;
        }

        public DbTranslatedChapterBuilder WithPictureId(ResourceId pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public DbTranslatedChapter Build()
        {
            return new DbTranslatedChapter
            {
                Id = _id,
                Level = _level,
                Name = _name,
                Description = _description,
                Languages = _languages,
                Pages = _pages,
                PictureId = _pictureId
            };
        }
    }
}
