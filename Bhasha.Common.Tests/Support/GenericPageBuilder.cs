using System;

namespace Bhasha.Common.Tests.Support
{
    public class GenericPageBuilder
    {
        private Guid _tokenId = Guid.NewGuid();
        private PageType _pageType = PageType.OneOutOfFour;
        private Guid[] _tipIds = new[] { Guid.NewGuid() };

        public GenericPageBuilder WithTokenId(Guid tokenId)
        {
            _tokenId = tokenId;
            return this;
        }

        public static GenericPageBuilder Default => new();

        public GenericPage Build()
        {
            return new GenericPage(
                _tokenId,
                _pageType,
                _tipIds);
        }
    }
}
