namespace Bhasha.Common
{
    public class Token
    {
        public string Label { get; }
        public Category[] Categories { get; set; }
        public LanguageLevel Level { get; set; }
        public TokenType TokenType { get; set; }
        public ResourceId? PictureId { get; set; }

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
