namespace Bhasha.Common.Tests.Support
{
    public class TokenIdBuilder
    {
        private int _groupId = 4;
        private int _sequenceNumber = 7;

        public static TokenIdBuilder Default = new TokenIdBuilder();
        public static TokenId Create() => Default.Build();

        public TokenIdBuilder WithGroupId(int groupId)
        {
            _groupId = groupId;
            return this;
        }

        public TokenIdBuilder WithSequenceNumber(int sequenceNumber)
        {
            _sequenceNumber = sequenceNumber;
            return this;
        }

        public TokenId Build()
        {
            return new TokenId(_groupId, _sequenceNumber);
        }
    }
}
