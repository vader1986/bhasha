namespace Bhasha.Common.Queries
{
    public class TranslationsCategoryQuery : TranslationsLevelQuery
    {
        public Category Category { get; }

        public TranslationsCategoryQuery(LanguageLevel level, Category category) : base(level)
        {
            Category = category;
        }
    }
}
