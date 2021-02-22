namespace Bhasha.Common.Queries
{
    public class TranslationsTokenTypeQuery : TranslationsCategoryQuery
    {
        public TokenType TokenType { get; }

        public TranslationsTokenTypeQuery(int maxItems, Language from, Language to, LanguageLevel level, Category category, TokenType tokenType) : base(maxItems, from, to, level, category)
        {
            TokenType = tokenType;
        }
    }
}
