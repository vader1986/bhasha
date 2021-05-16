using System;
using System.Collections.Generic;
using System.Linq;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Database representation of an <see cref="Expression"/> including references
    /// to all available translations. 
    /// </summary>
    public class DbExpression : ICanBeValidated, IEntity, IEquatable<DbExpression?>
    {
        /// <summary>
        /// Unqiue identifier for the expression.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Type of the expression.
        /// </summary>
        public ExpressionType ExprType { get; set; }

        /// <summary>
        /// Common European Framework of Reference (CEFR) for this expression.
        /// </summary>
        public CEFR Cefr { get; set; }

        /// <summary>
        /// Translations of the expression. Keys are <see cref="Language"/> IDs,
        /// values are arrays of <see cref="Word"/> IDs. 
        /// </summary>
        public Dictionary<string, DbWords>? Translations { get; set; }

        public void Validate()
        {
            if (Translations == null || Translations.Count == 0)
            {
                throw new InvalidObjectException(this);
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbExpression);
        }

        public bool Equals(DbExpression? other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   ExprType == other.ExprType &&
                   Cefr == other.Cefr &&
                   Translations.SequenceEqual(other.Translations);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ExprType, Cefr, Translations);
        }

        public static bool operator ==(DbExpression? left, DbExpression? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbExpression? left, DbExpression? right)
        {
            return !(left == right);
        }
    }
}
