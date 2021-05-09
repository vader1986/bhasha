using System;
using System.Linq;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    public class DbWords : ICanBeValidated, IEquatable<DbWords?>
    {
        /// <summary>
        /// List of <see cref="Word.Id"/> for this word list.
        /// </summary>
        public Guid[]? WordIds { get; set; }

        public void Validate()
        {
            if (WordIds == null || WordIds.Length == 0)
            {
                throw new InvalidObjectException(this);
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbWords);
        }

        public bool Equals(DbWords? other)
        {
            return other != null && WordIds.SequenceEqual(other.WordIds);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WordIds);
        }

        public static bool operator ==(DbWords? left, DbWords? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbWords? left, DbWords? right)
        {
            return !(left == right);
        }
    }
}
