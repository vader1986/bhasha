namespace Bhasha.Common.Queries
{
    public class TranslationsTokenTypeQuery : TranslationsCategoryQuery
    {
        public TokenType TokenType { get; }

        public TranslationsTokenTypeQuery(Language from, Language to, LanguageLevel level, Category category, TokenType tokenType) : base(from, to, level, category)
        {
            TokenType = tokenType;
        }
    }
}
