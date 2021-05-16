using System;
using System.Collections.Generic;
using System.Linq;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    public class DbTranslatedExpression : ICanBeValidated, IEquatable<DbTranslatedExpression?>
    {
        /// <summary>
        /// Reference to the <see cref="DbExpression"/>. 
        /// </summary>
        public Guid ExpressionId { get; set; }

        /// <summary>
        /// Type of the expression.
        /// </summary>
        public ExpressionType ExprType { get; set; }

        /// <summary>
        /// Common European Framework of Reference (CEFR) for this expression.
        /// </summary>
        public CEFR Cefr { get; set; }

        /// <summary>
        /// Translated words of the expression.
        /// </summary>
        public DbTranslatedWord[]? Words { get; set; }

        public void Validate()
        {
            if (Words == null || Words.Length == 0)
            {
                throw new InvalidObjectException(this);
            }

            foreach (var word in Words)
            {
                word.Validate();
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbTranslatedExpression);
        }

        public bool Equals(DbTranslatedExpression? other)
        {
            return other != null &&
                   ExpressionId.Equals(other.ExpressionId) &&
                   ExprType == other.ExprType &&
                   Cefr == other.Cefr &&
                   Words.SequenceEqual(other.Words);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ExpressionId, ExprType, Cefr, Words);
        }

        public static bool operator ==(DbTranslatedExpression? left, DbTranslatedExpression? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbTranslatedExpression? left, DbTranslatedExpression? right)
        {
            return !(left == right);
        }
    }
}
