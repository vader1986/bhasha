using System;
using System.Collections.Generic;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    public class DbTranslatedPage : ICanBeValidated, IEquatable<DbTranslatedPage?>
    {
        /// <summary>
        /// Type of page used to learn a new part of speech. 
        /// </summary>
        public PageType PageType { get; set; }

        /// <summary>
        /// Expression translated into native language.
        /// </summary>
        public DbTranslatedExpression? Native { get; set; }

        /// <summary>
        /// Expression translated into target language.
        /// </summary>
        public DbTranslatedExpression? Target { get; set; }

        public void Validate()
        {
            if (Native == null || Target == null)
            {
                throw new InvalidObjectException(this);
            }

            Native.Validate();
            Target.Validate();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbTranslatedPage);
        }

        public bool Equals(DbTranslatedPage? other)
        {
            return other != null &&
                   PageType == other.PageType &&
                   Native == other.Native &&
                   Target == other.Target;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PageType, Native, Target);
        }

        public static bool operator ==(DbTranslatedPage? left, DbTranslatedPage? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbTranslatedPage? left, DbTranslatedPage? right)
        {
            return !(left == right);
        }
    }
}
