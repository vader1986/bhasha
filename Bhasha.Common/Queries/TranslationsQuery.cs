namespace Bhasha.Common.Queries
{
    public abstract class TranslationsQuery : IQuery
    {
        public int MaxItems { get; }
        public Language From { get; }
        public Language To { get; }

        protected TranslationsQuery(int maxItems, Language from, Language to)
        {
            MaxItems = maxItems;
            From = from;
            To = to;
        }
    }

    public abstract class TranslationsLevelQuery : TranslationsQuery
    {
        public LanguageLevel Level { get; }

        protected TranslationsLevelQuery(int maxItems, Language from, Language to, LanguageLevel level) : base(maxItems, from, to)
        {
            Level = level;
        }
    }
}
