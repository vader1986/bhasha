using System;

namespace Bhasha.Common
{
    public class GenericPage
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
    }
}
