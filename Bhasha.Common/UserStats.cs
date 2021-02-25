using System.Collections.Generic;

namespace Bhasha.Common
{
    public class UserStats
    {
        public int GroupId { get; }
        public int CompletedTokens { get; }
        public int CompletedChapters { get; }
        public ISet<int> CompletedSequenceNumbers { get; }

        public UserStats(int groupId, int completedTokens, int completedChapters, ISet<int> completedSequenceNumbers)
        {
            GroupId = groupId;
            CompletedTokens = completedTokens;
            CompletedChapters = completedChapters;
            CompletedSequenceNumbers = completedSequenceNumbers;
        }
    }
}
