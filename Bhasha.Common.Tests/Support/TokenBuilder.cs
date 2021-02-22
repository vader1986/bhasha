namespace Bhasha.Common.Tests.Support
{
    public class TokenBuilder
    {
        private string _label = "cat";
        private Category[] _categories = new[] { new Category("pet"), new Category("animal") };
        private LanguageLevel _level = LanguageLevel.A1;
        private TokenType _tokenType = TokenType.Noun;
        private ResourceId _pictureId;

        public static TokenBuilder Default => new TokenBuilder();
        public static Token Create() => Default.Build();

        public TokenBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public TokenBuilder WithCategories(Category[] categories)
        {
            _categories = categories;
            return this;
        }

        public TokenBuilder WithLevel(LanguageLevel level)
        {
            _level = level;
            return this;
        }

        public TokenBuilder WithTokenType(TokenType tokenType)
        {
            _tokenType = tokenType;
            return this;
        }

        public TokenBuilder WithPictureId(ResourceId pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public Token Build()
        {
            return new Token(_label, _categories, _level, _tokenType, _pictureId);
        }
    }
}
