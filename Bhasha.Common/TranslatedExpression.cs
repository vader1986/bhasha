using System;
using System.Linq;

namespace Bhasha.Common
{
    public class TranslatedExpression : IEquatable<TranslatedExpression?>
    {
        /// <summary>
        /// Language independent description of the expression to translate.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// Translated words of the expression.
        /// </summary>
        public TranslatedWord[] Words { get; }

        /// <summary>
        /// Translated expression in native script. 
        /// </summary>
        public string Native { get; }

        /// <summary>
        /// Translated expression in spoken script.
        /// </summary>
        public string Spoken { get; }

        public TranslatedExpression(Expression expression, TranslatedWord[] words, string native, string spoken)
        {
            Expression = expression;
            Words = words;
            Native = native;
            Spoken = spoken;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TranslatedExpression);
        }

        public bool Equals(TranslatedExpression? other)
        {
            return other != null && Expression == other.Expression && Words.SequenceEqual(other.Words) && Native == other.Native && Spoken == other.Spoken;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Expression, Words, Native, Spoken);
        }

        public static bool operator ==(TranslatedExpression? left, TranslatedExpression? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TranslatedExpression? left, TranslatedExpression? right)
        {
            return !(left == right);
        }
    }
}
