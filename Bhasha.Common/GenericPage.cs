using System;

namespace Bhasha.Common
{
    public class GenericPage : IEquatable<GenericPage>
    {
        /// <summary>
        /// Reference to the generic token this page is designed for. 
        /// </summary>
        public Guid TokenId { get; }

        /// <summary>
        /// Type of page.
        /// </summary>
        public PageType PageType { get; }

        public GenericPage(Guid tokenId, PageType pageType)
        {
            TokenId = tokenId;
            PageType = pageType;
        }

        public override string ToString()
        {
            return $"{nameof(TokenId)}: {TokenId}, {nameof(PageType)}: {PageType}";
        }

        public bool Equals(GenericPage other)
        {
            return other != null && other.TokenId == TokenId && other.PageType == PageType;
        }
    }
}
