using System;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    public class DbTranslatedExpression : ICanBeValidated
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
            if (Words == null || Words.Length == 0)
            {
                throw new InvalidObjectException(this);
            }

            foreach (var word in Words)
            {
                word.Validate();
            }
        }
    }
}
