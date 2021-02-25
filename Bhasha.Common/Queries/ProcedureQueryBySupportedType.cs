namespace Bhasha.Common.Queries
{
    public class ProcedureQueryBySupportedType : ProcedureQuery
    {
        public TokenType SupportedType { get; }

        public ProcedureQueryBySupportedType(int maxItems, TokenType supportedType) : base(maxItems)
        {
            SupportedType = supportedType;
        }
    }
}
