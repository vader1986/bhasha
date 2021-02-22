namespace Bhasha.Common.Queries
{
    public class TranslationsLabelQuery : TranslationsQuery
    {
        public string Label { get; }

        public TranslationsLabelQuery(int maxItems, Language from, Language to, string label) : base(maxItems, from, to)
        {
            Label = label;
        }
    }
}
