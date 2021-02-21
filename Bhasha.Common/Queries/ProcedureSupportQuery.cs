namespace Bhasha.Common.Queries
{
    public class ProcedureSupportQuery : ProcedureQuery
    {
        public TokenType SupportedType { get; }

        public ProcedureSupportQuery(TokenType supportedType)
        {
            SupportedType = supportedType;
        }
    }
}
