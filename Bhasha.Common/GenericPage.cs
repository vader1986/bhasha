using System;
using System.Linq;

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

        /// <summary>
        /// Tips for the current page.
        /// </summary>
        public Guid[] TipIds { get; }

        public GenericPage(Guid tokenId, PageType pageType, Guid[] tipIds)
        {
            TokenId = tokenId;
            PageType = pageType;
            TipIds = tipIds;
        }

        public override string ToString()
        {
            return $"{nameof(TokenId)}: {TokenId}, {nameof(PageType)}: {PageType}, {nameof(TipIds)}: {string.Join("/", TipIds)}";
        }

        public bool Equals(GenericPage other)
        {
            return other != null && other.TokenId == TokenId && other.PageType == PageType && other.TipIds.SequenceEqual(TipIds);
        }
    }
}
