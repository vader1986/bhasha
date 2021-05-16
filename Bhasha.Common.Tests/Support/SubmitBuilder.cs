using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class SubmitBuilder
    {
        private Guid _chapterId = Guid.NewGuid();
        private int _pageIndex = Rnd.Create.Next(0, 5);
        private string _solution = Rnd.Create.NextString();

        public static SubmitBuilder Default => new();

        public SubmitBuilder WithChapterId(Guid chapterId)
        {
            _chapterId = chapterId;
            return this;
        }

        public SubmitBuilder WithPageIndex(int pageIndex)
        {
            _pageIndex = pageIndex;
            return this;
        }

        public SubmitBuilder WithSolution(string solution)
        {
            _solution = solution;
            return this;
        }

        public Submit Build()
        {
            return new Submit(_chapterId, _pageIndex, _solution);
        }
    }
}
