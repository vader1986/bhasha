namespace Bhasha.Common
{
    public class TokenId
    {
        public int GroupId { get; }
        public int SequenceNumber { get; }

        public TokenId(int groupId, int sequenceNumber)
        {
            GroupId = groupId;
            SequenceNumber = sequenceNumber;
        }
    }
}
