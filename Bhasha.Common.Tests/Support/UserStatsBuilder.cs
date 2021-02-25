using System.Collections.Generic;

namespace Bhasha.Common.Tests.Support
{
    public class UserStatsBuilder
    {
        private int _groupId = 4;
        private int _completedTokens = 99;
        private int _completedChapters = 17;
        private ISet<int> _completedSequenceNumbers = new HashSet<int> { 1, 2, 6 };
        private LanguageLevel _level = LanguageLevel.B2;

        public static UserStatsBuilder Default = new UserStatsBuilder();
        public static UserStats Create() => Default.Build();

        public UserStatsBuilder WithGroupId(int groupId)
        {
            _groupId = groupId;
            return this;
        }

        public UserStatsBuilder WithCompletedTokens(int completedTokens)
        {
            _completedTokens = completedTokens;
            return this;
        }

        public UserStatsBuilder WithCompletedChapters(int completedChapters)
        {
            _completedChapters = completedChapters;
            return this;
        }

        public UserStatsBuilder WithCompletedSequenceNumbers(ISet<int> completedSequenceNumbers)
        {
            _completedSequenceNumbers = completedSequenceNumbers;
            return this;
        }

        public UserStatsBuilder WithLevel(LanguageLevel level)
        {
            _level = level;
            return this;
        }

        public UserStats Build()
        {
            return new UserStats(_groupId, _completedTokens, _completedChapters, _completedSequenceNumbers, _level);
        }
    }
}
