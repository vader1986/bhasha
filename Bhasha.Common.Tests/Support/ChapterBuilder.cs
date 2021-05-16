using System;
using System.Linq;

namespace Bhasha.Common.Tests.Support
{
    public class ChapterBuilder
    {
        private Guid _id = Guid.NewGuid();
        private int _level = Rnd.Create.Next(1, 10);
        private TranslatedExpression _name = TranslatedExpressionBuilder.Default.Build();
        private TranslatedExpression _description = TranslatedExpressionBuilder.Default.Build();
        private Page[] _pages = Enumerable.Range(2, 5).Select(_ => PageBuilder.Default.Build()).ToArray();
        private ResourceId _pictureId;

        public static ChapterBuilder Default => new();

        public ChapterBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ChapterBuilder WithLevel(int level)
        {
            _level = level;
            return this;
        }

        public ChapterBuilder WithName(TranslatedExpression name)
        {
            _name = name;
            return this;
        }

        public ChapterBuilder WithDescription(TranslatedExpression description)
        {
            _description = description;
            return this;
        }

        public ChapterBuilder WithPages(Page[] pages)
        {
            _pages = pages;
            return this;
        }

        public ChapterBuilder WithPictureId(ResourceId pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public Chapter Build()
        {
            return new Chapter(_id, _level, _name, _description, _pages, _pictureId);
        }
    }
}
