namespace Bhasha.Common.Queries
{
    public abstract class TranslationQuery
    {
        public Language From { get; }
        public Language To { get; }

        protected TranslationQuery(Language from, Language to)
        {
            From = from;
            To = to;
        }
    }
}
