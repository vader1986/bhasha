namespace Bhasha.Common.Queries
{
    public class ProcedureIdQuery : ProcedureQuery
    {
        public ProcedureId Id { get; }

        public ProcedureIdQuery(ProcedureId id)
        {
            Id = id;
        }
    }
}
