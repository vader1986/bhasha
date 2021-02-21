namespace Bhasha.Common.Queries
{
    public class TranslationsLabelQuery : TranslationsQuery
    {
        public string Label { get; }

        public TranslationsLabelQuery(string label)
        {
            Label = label;
        }
    }
}
