using System;

namespace Bhasha.Common
{
    public class TranslatedWord : IEquatable<TranslatedWord?>
    {
        /// <summary>
        /// Language independent description of the word.
        /// </summary>
        public Word Word { get; }

        /// <summary>
        /// Word translated into <see cref="Language"/> written in its native script.
        /// </summary>
        public string Native { get; }

        /// <summary>
        /// Word translated into <see cref="Language"/> written in spoken language. 
        /// </summary>
        public string Spoken { get; }

        /// <summary>
        /// Identifier of the audio file for the word.
        /// </summary>
        public ResourceId? AudioId { get; }

        public TranslatedWord(Word word, string native, string spoken, ResourceId? audioId)
        {
            Word = word;
            Native = native;
            Spoken = spoken;
            AudioId = audioId;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TranslatedWord);
        }

        public bool Equals(TranslatedWord? other)
        {
            return other != null && Word == other.Word && Native == other.Native && Spoken == other.Spoken && Equals(AudioId, other.AudioId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Word, Native, Spoken, AudioId);
        }

        public static bool operator ==(TranslatedWord? left, TranslatedWord? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TranslatedWord? left, TranslatedWord? right)
        {
            return !(left == right);
        }
    }
}
