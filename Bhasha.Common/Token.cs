namespace Bhasha.Common
{
    public class Token
    {
        public string Label { get; }
        public Category[] Categories { get; }
        public LanguageLevel Level { get; }
        public TokenType TokenType { get; }
        public ResourceId? PictureId { get; }

        public Token(string label, Category[] categories, LanguageLevel level, TokenType tokenType, ResourceId? pictureId = default)
        {
            Label = label;
            Categories = categories;
            Level = level;
            TokenType = tokenType;
            PictureId = pictureId;
        }
    }
}
