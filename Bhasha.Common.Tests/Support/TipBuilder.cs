using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class TipBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _chapterId = Guid.NewGuid();
        private int _pageIndex = Rnd.Create.Next();
        private string _text = Rnd.Create.NextString();

        public TipBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public TipBuilder WithChapterId(Guid chapterId)
        {
            _chapterId = chapterId;
            return this;
        }

        public TipBuilder WithPageIndex(int pageIndex)
        {
            _pageIndex = pageIndex;
            return this;
        }

        public static TipBuilder Default => new();

        public Tip Build()
        {
            return new Tip(
                _id,
                _chapterId,
                _pageIndex,
                _text);
        }
    }
}
