namespace Bhasha.Common
{
    public class Token
    {
        /// <summary>
        /// Unqiue identifier of the token.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Language level associated with the token. The level indicates the
        /// complexity of the token. 
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// English representation of the token.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Common European Framework of Reference (CEFR) for this token.
        /// </summary>
        public CEFR Cefr { get; }

        /// <summary>
        /// Part of speech of this token.
        /// </summary>
        public TokenType TokenType { get; }

        /// <summary>
        /// Categories associated with this token. 
        /// </summary>
        public Category[] Categories { get; }

        /// <summary>
        /// Link to an image representation for this token (optional).
        /// </summary>
        public ResourceId? PictureId { get; }

        public Token(int id, int level, string label, CEFR cefr, TokenType tokenType, Category[] categories, ResourceId? pictureId = default)
        {
            Id = id;
            Level = level;
            Label = label;
            Cefr = cefr;
            TokenType = tokenType;
            Categories = categories;
            PictureId = pictureId;
        }
    }
}
