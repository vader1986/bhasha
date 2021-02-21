namespace Bhasha.Common.Queries
{
    public abstract class TranslationsQuery
    {
        public Language From { get; }
        public Language To { get; }

        protected TranslationsQuery(Language from, Language to)
        {
            From = from;
            To = to;
        }
    }

    public abstract class TranslationsLevelQuery : TranslationsQuery
    {
        public LanguageLevel Level { get; }

        protected TranslationsLevelQuery(Language from, Language to, LanguageLevel level) : base(from, to)
        {
            Level = level;
        }
    }
}
