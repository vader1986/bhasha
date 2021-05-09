using System;
using System.Collections.Generic;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    /// <summary>
    /// Database representation for a <see cref="Word"/> and all its translations.
    /// </summary>
    public class DbWord : ICanBeValidated, IEntity
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
    }
}
