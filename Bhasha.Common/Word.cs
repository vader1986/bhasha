using System;

namespace Bhasha.Common
{
    public class Word : IEquatable<Word?>
    {
        /// <summary>
        /// Unqiue identifier of the word (language independent).
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Part of speech of the word.
        /// </summary>
        public PartOfSpeech PartOfSpeech { get; }

        /// <summary>
        /// Common European Framework of Reference (CEFR) for this word.
        /// </summary>
        public CEFR Cefr { get; }

        /// <summary>
        /// Optional picture representing the word.
        /// </summary>
        public ResourceId? PictureId { get; }

        public Word(Guid id, PartOfSpeech partOfSpeech, CEFR cefr, ResourceId? pictureId)
        {
            Id = id;
            PartOfSpeech = partOfSpeech;
            Cefr = cefr;
            PictureId = pictureId;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Word);
        }

        public bool Equals(Word? other)
        {
            return other != null && Id.Equals(other.Id) && PartOfSpeech == other.PartOfSpeech && Cefr == other.Cefr && Equals(PictureId, other.PictureId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, PartOfSpeech, Cefr, PictureId);
        }

        public static bool operator ==(Word? left, Word? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Word? left, Word? right)
        {
            return !(left == right);
        }
    }
}
