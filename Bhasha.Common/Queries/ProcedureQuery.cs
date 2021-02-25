namespace Bhasha.Common.Queries
{
    public abstract class ProcedureQuery
    {
        public int MaxItems { get; }

        protected ProcedureQuery(int maxItems)
        {
            MaxItems = maxItems;
        }
    }
}
