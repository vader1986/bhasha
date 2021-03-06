namespace Bhasha.Common
{
    public class LanguageToken
    {
        /// <summary>
        /// Language of the translation.
        /// </summary>
        public Language Language { get; }

        /// <summary>
        /// Translation in the native script of the language.
        /// </summary>
        public string Native { get; }

        /// <summary>
        /// Translation in IPA (International Phonetic Alphabet).
        /// </summary>
        public string Spoken { get; }

        /// <summary>
        /// Optional link to an audio file.
        /// </summary>
        public ResourceId? AudioId { get; }

        public LanguageToken(Language language, string native, string spoken, ResourceId? audioId = default)
        {
            Language = language;
            Native = native;
            Spoken = spoken;
            AudioId = audioId;
        }
    }
}
