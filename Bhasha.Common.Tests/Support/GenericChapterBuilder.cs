using System;
using System.Linq;

namespace Bhasha.Common.Tests.Support
{
    public class GenericChapterBuilder
    {
        private Guid _id = Guid.NewGuid();
        private int _level = Rnd.Create.Next();
        private Guid _nameId = Guid.NewGuid();
        private Guid _descriptionId = Guid.NewGuid();
        private GenericPage[] _pages = Enumerable.Range(1, 5).Select(_ => GenericPageBuilder.Default.Build()).ToArray();

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
                _nameId,
                _descriptionId,
                _pages);
        }
    }
}
