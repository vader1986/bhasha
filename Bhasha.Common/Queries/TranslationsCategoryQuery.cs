namespace Bhasha.Common.Queries
{
    public class TranslationsCategoryQuery : TranslationsLevelQuery
    {
        public Category Category { get; }

        public TranslationsCategoryQuery(Language from, Language to, LanguageLevel level, Category category) : base(from, to, level)
        {
            Category = category;
        }
    }
}
