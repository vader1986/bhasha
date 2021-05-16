using System;
using System.Collections.Generic;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Translation of a single <see cref="Word"/>. 
    /// </summary>
    public class DbTranslation : ICanBeValidated, IEquatable<DbTranslation?>
    {
        /// <summary>
        /// Word translated into <see cref="Language"/> written in its native script.
        /// </summary>
        public string? Native { get; set; }

        /// <summary>
        /// Word translated into <see cref="Language"/> written in spoken language. 
        /// </summary>
        public string? Spoken { get; set; }

        /// <summary>
        /// Identifier of the audio file for the word.
        /// </summary>
        public ResourceId? AudioId { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Native) || string.IsNullOrWhiteSpace(Spoken))
            {
                throw new InvalidObjectException(this);
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbTranslation);
        }

        public bool Equals(DbTranslation? other)
        {
            return other != null &&
                   Native == other.Native &&
                   Spoken == other.Spoken &&
                   AudioId == other.AudioId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Native, Spoken, AudioId);
        }

        public static bool operator ==(DbTranslation? left, DbTranslation? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbTranslation? left, DbTranslation? right)
        {
            return !(left == right);
        }
    }
}
