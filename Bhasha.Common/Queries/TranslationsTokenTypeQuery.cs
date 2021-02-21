namespace Bhasha.Common.Queries
{
    public class TranslationsTokenTypeQuery : TranslationsCategoryQuery
    {
        public TokenType TokenType { get; }

        public TranslationsTokenTypeQuery(LanguageLevel level, Category category, TokenType tokenType) : base(level, category)
        {
            TokenType = tokenType;
        }
    }
}
