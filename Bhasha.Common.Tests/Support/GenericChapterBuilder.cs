using System;
using System.Linq;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class GenericChapterBuilder
    {
        private Guid _id = Guid.NewGuid();
        private int _level = Rnd.Create.Next();
        private string _name = Rnd.Create.NextString();
        private string _description = Rnd.Create.NextPhrase();
        private GenericPage[] _pages = Enumerable.Range(1, 5).Select(_ => GenericPageBuilder.Default.Build()).ToArray();
        private ResourceId _pictureId = Rnd.Create.NextString();

        public GenericChapterBuilder WithPictureId(ResourceId pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public GenericChapterBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public GenericChapterBuilder WithPages(GenericPage[] pages)
        {
            _pages = pages;
            return this;
        }

        public static GenericChapterBuilder Default => new();

        public GenericChapter Build()
        {
            return new GenericChapter(
                _id,
                _level,
                _name,
                _description,
                _pages,
                _pictureId);
        }
    }
}
