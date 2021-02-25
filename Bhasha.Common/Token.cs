namespace Bhasha.Common
{
    public class Token
    {
        public TokenId Id { get; }
        public string Label { get; }
        public LanguageLevel Level { get; }
        public TokenType TokenType { get; }
        public Category[] Categories { get; }
        public ResourceId? PictureId { get; }

        public Token(TokenId id, string label, LanguageLevel level, TokenType tokenType, Category[] categories, ResourceId? pictureId = default)
        {
            Id = id;
            Label = label;
            Level = level;
            TokenType = tokenType;
            Categories = categories;
            PictureId = pictureId;
        }
    }
}
