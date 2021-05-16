using System;

namespace Bhasha.Common
{
    public class Expression : IEquatable<Expression?>
    {
        /// <summary>
        /// Unqiue identifier for the expression.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Type of the expression.
        /// </summary>
        public ExpressionType ExprType { get; }

        /// <summary>
        /// Common European Framework of Reference (CEFR) for this expression.
        /// </summary>
        public CEFR Cefr { get; }

        public Expression(Guid id, ExpressionType exprType, CEFR cefr)
        {
            Id = id;
            ExprType = exprType;
            Cefr = cefr;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(ExprType)}: {ExprType}, {nameof(Cefr)}: {Cefr}";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Expression);
        }

        public bool Equals(Expression? other)
        {
            return other != null && Id.Equals(other.Id) && ExprType == other.ExprType && Cefr == other.Cefr;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ExprType, Cefr);
        }

        public static bool operator ==(Expression? left, Expression? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Expression? left, Expression? right)
        {
            return !(left == right);
        }
    }
}
