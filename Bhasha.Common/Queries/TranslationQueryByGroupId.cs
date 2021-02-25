namespace Bhasha.Common.Queries
{
    public class TranslationQueryByGroupId : TranslationQuery
    {
        public int GroupId { get; }
        public TokenType TokenType { get; }

        public TranslationQueryByGroupId(Language from, Language to, int groupId, TokenType tokenType) : base(from, to)
        {
            GroupId = groupId;
            TokenType = tokenType;
        }
    }
}
