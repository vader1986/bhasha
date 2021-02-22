namespace Bhasha.Common.Queries
{
    public class TranslationsCategoryQuery : TranslationsLevelQuery
    {
        public Category Category { get; }

        public TranslationsCategoryQuery(int maxItems, Language from, Language to, LanguageLevel level, Category category) : base(maxItems, from, to, level)
        {
            Category = category;
        }
    }
}
