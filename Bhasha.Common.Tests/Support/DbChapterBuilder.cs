using System;
using System.Linq;
using Bhasha.Common.Database;

namespace Bhasha.Common.Tests.Support
{
    public class DbChapterBuilder
    {
        private Guid _id = Guid.NewGuid();
        private int _level = Rnd.Create.Next();
        private Guid _nameId = Guid.NewGuid();
        private Guid _descriptionId = Guid.NewGuid();
        private DbPage[] _pages = Enumerable.Range(1, 5).Select(_ => DbPageBuilder.Default.Build()).ToArray();
        private ResourceId _pictureId;

        public static DbChapterBuilder Default => new();

        public DbChapterBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public DbChapterBuilder WithLevel(int level)
        {
            _level = level;
            return this;
        }

        public DbChapterBuilder WithPages(DbPage[] pages)
        {
            _pages = pages;
            return this;
        }

        public DbChapterBuilder WithPictureId(ResourceId pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public DbChapter Build()
        {
            return new DbChapter {
                Id = _id,
                Level = _level,
                NameId = _nameId,
                DescriptionId = _descriptionId,
                Pages = _pages,
                PictureId = _pictureId
            };
        }
    }
}
