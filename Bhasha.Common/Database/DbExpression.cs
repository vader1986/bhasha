using System;
using System.Collections.Generic;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Database representation of an <see cref="Expression"/> including references
    /// to all available translations. 
    /// </summary>
    public class DbExpression : ICanBeValidated, IEntity
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
        public Dictionary<string, Guid[]>? Translations { get; set; }

        public void Validate()
        {
            if (Translations == null || Translations.Count == 0)
            {
                throw new InvalidObjectException(this);
            }
        }
    }
}
