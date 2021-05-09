using System;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Represents a <see cref="Page"/> by only holding the minimum amount of
    /// information, references (IDs) rather than actual objects.
    /// </summary>
    public class DbPage
    {
        /// <summary>
        /// Type of page used to learn a new part of speech. 
        /// </summary>
        public PageType PageType { get; set; }

        /// <summary>
        /// Reference to the <see cref="Expression"/> to learn from this page.
        /// </summary>
        public Guid ExpressionId { get; set; }
    }
}
