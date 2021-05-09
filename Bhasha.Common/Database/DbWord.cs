using System;
using System.Collections.Generic;
using System.Linq;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Database representation for a <see cref="Word"/> and all its translations.
    /// </summary>
    public class DbWord : ICanBeValidated, IEntity, IEquatable<DbWord?>
    {
        /// <summary>
        /// Unqiue identifier of the word (language independent).
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Part of speech of the word.
        /// </summary>
        public PartOfSpeech PartOfSpeech { get; set; }

        /// <summary>
        /// Common European Framework of Reference (CEFR) for this word.
        /// </summary>
        public CEFR Cefr { get; set; }

        /// <summary>
        /// Optional picture representing the word.
        /// </summary>
        public ResourceId? PictureId { get; set; }

        /// <summary>
        /// Translations of the word.
        /// </summary>
        public Dictionary<string, DbTranslation>? Translations { get; set; }

        public void Validate()
        {
            if (Translations == null || Translations.Count == 0)
            {
                throw new InvalidObjectException(this);
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbWord);
        }

        public bool Equals(DbWord? other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   PartOfSpeech == other.PartOfSpeech &&
                   Cefr == other.Cefr &&
                   PictureId == other.PictureId &&
                   Translations.SequenceEqual(other.Translations);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, PartOfSpeech, Cefr, PictureId, Translations);
        }

        public static bool operator ==(DbWord? left, DbWord? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbWord? left, DbWord? right)
        {
            return !(left == right);
        }
    }
}
