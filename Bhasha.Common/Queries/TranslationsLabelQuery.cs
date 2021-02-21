namespace Bhasha.Common.Queries
{
    public class TranslationsLabelQuery : TranslationsQuery
    {
        public string Label { get; }

        public TranslationsLabelQuery(Language from, Language to, string label) : base(from, to)
        {
            Label = label;
        }
    }
}
