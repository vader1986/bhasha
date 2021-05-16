using System;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Represents a <see cref="Page"/> by only holding the minimum amount of
    /// information, references (IDs) rather than actual objects.
    /// </summary>
    public class DbPage : IEquatable<DbPage?>
    {
        /// <summary>
        /// Type of page used to learn a new part of speech. 
        /// </summary>
        public PageType PageType { get; set; }

        /// <summary>
        /// Reference to the <see cref="Expression"/> to learn from this page.
        /// </summary>
        public Guid ExpressionId { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbPage);
        }

        public bool Equals(DbPage? other)
        {
            return other != null &&
                   PageType == other.PageType &&
                   ExpressionId.Equals(other.ExpressionId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageType, ExpressionId);
        }

        public static bool operator ==(DbPage? left, DbPage? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbPage? left, DbPage? right)
        {
            return !(left == right);
        }
    }
}
