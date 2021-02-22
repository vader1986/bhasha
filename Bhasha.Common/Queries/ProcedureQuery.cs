namespace Bhasha.Common.Queries
{
    public abstract class ProcedureQuery : IQuery
    {
        public int MaxItems { get; }

        protected ProcedureQuery(int maxItems)
        {
            MaxItems = maxItems;
        }
    }
}
