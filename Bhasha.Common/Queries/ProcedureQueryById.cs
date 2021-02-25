namespace Bhasha.Common.Queries
{
    public class ProcedureQueryById : ProcedureQuery
    {
        public ProcedureId Id { get; }

        public ProcedureQueryById(int maxItems, ProcedureId id) : base(maxItems)
        {
            Id = id;
        }
    }
}
