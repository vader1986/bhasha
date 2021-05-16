using System;
using System.Collections.Generic;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    public class DbTranslatedWord : ICanBeValidated, IEquatable<DbTranslatedWord?>
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
        /// Translation of the word.
        /// </summary>
        public DbTranslation? Translation { get; set; }

        public void Validate()
        {
            if (Translation == null)
            {
                throw new InvalidObjectException(this);
            }

            Translation.Validate();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DbTranslatedWord);
        }

        public bool Equals(DbTranslatedWord? other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   PartOfSpeech == other.PartOfSpeech &&
                   Cefr == other.Cefr &&
                   PictureId == other.PictureId &&
                   Translation == other.Translation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, PartOfSpeech, Cefr, PictureId, Translation);
        }

        public static bool operator ==(DbTranslatedWord? left, DbTranslatedWord? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DbTranslatedWord? left, DbTranslatedWord? right)
        {
            return !(left == right);
        }
    }
}
