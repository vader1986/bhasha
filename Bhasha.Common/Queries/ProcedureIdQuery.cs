namespace Bhasha.Common.Queries
{
    public class ProcedureIdQuery : ProcedureQuery
    {
        public ProcedureId Id { get; }

        public ProcedureIdQuery(int maxItems, ProcedureId id) : base(maxItems)
        {
            Id = id;
        }
    }
}
