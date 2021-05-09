using System;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Database
{
    public class DbTranslatedWord : ICanBeValidated
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
    }
}
