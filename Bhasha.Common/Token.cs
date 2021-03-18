using System;

namespace Bhasha.Common
{
    public class Token : IEntity
    {
        /// <summary>
        /// Unqiue identifier of the token.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// English representation of the token.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Language level associated with the token. The level indicates the
        /// complexity of the token. 
        /// </summary>
        public int Level { get; }

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
        public string[] Categories { get; }

        /// <summary>
        /// Link to an image representation for this token (optional).
        /// </summary>
        public ResourceId? PictureId { get; }

        public Token(Guid id, string label, int level, CEFR cefr, TokenType tokenType, string[] categories, ResourceId? pictureId = default)
        {
            Id = id;
            Label = label;
            Level = level;
            Cefr = cefr;
            TokenType = tokenType;
            Categories = categories;
            PictureId = pictureId;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Label)}: {Label}, {nameof(Level)}: {Level}, {nameof(Cefr)}: {Cefr}, {nameof(TokenType)}: {TokenType}, {nameof(Categories)}: {string.Join("/", Categories)}, {nameof(PictureId)}: {PictureId}";
        }
    }
}
