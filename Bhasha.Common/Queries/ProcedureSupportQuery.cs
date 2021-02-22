namespace Bhasha.Common.Queries
{
    public class ProcedureSupportQuery : ProcedureQuery
    {
        public TokenType SupportedType { get; }

        public ProcedureSupportQuery(int maxItems, TokenType supportedType) : base(maxItems)
        {
            SupportedType = supportedType;
        }
    }
}
