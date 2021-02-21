namespace Bhasha.Common.Queries
{
    public abstract class TranslationsQuery
    {
    }

    public abstract class TranslationsLevelQuery : TranslationsQuery
    {
        public LanguageLevel Level { get; }

        protected TranslationsLevelQuery(LanguageLevel level)
        {
            Level = level;
        }
    }
}
