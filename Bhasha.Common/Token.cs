#nullable enable
namespace Bhasha.Common
{
    public class Token
    {
        public string Label { get; }
        public string[] Categories { get; set; }
        public LanguageLevel Level { get; set; }
        public TokenType TokenType { get; set; }
        public ResourceId? PictureId { get; set; }

        public Token(string label, string[] categories, LanguageLevel level, TokenType tokenType, ResourceId? pictureId = default)
        {
            Label = label;
            Categories = categories;
            Level = level;
            TokenType = tokenType;
            PictureId = pictureId;
        }
    }
}
