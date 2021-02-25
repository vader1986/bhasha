using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class UserProgressDtoBuilder
    {
        private string _userId = "User-123";
        private string _from = Languages.English;
        private string _to = Languages.Bengoli;
        private string _level = LanguageLevel.B2.ToString();
        private int _groupId = 3;
        private int _completedChapters = 5;
        private int _completedTokens = 112;
        private int[] _completedSequenceNumbers = new[] { 1, 3, 4, 5 };

        public static UserProgressDtoBuilder Default = new UserProgressDtoBuilder();
        public static UserProgressDto Create() => Default.Build();

        public UserProgressDtoBuilder WithUserId(string userId)
        {
            _userId = userId;
            return this;
        }

        public UserProgressDtoBuilder WithFrom(string from)
        {
            _from = from;
            return this;
        }

        public UserProgressDtoBuilder WithTo(string to)
        {
            _to = to;
            return this;
        }

        public UserProgressDtoBuilder WithLevel(string level)
        {
            _level = level;
            return this;
        }

        public UserProgressDtoBuilder WithGroupId(int groupId)
        {
            _groupId = groupId;
            return this;
        }

        public UserProgressDtoBuilder WithCompletedChapters(int completedChapters)
        {
            _completedChapters = completedChapters;
            return this;
        }

        public UserProgressDtoBuilder WithCompletedTokens(int completedTokens)
        {
            _completedTokens = completedTokens;
            return this;
        }

        public UserProgressDtoBuilder WithCompletedSequenceNumbers(int[] completedSequenceNumbers)
        {
            _completedSequenceNumbers = completedSequenceNumbers;
            return this;
        }

        public UserProgressDto Build()
        {
            return new UserProgressDto
            {
                UserId = _userId,
                From = _from,
                To = _to,
                Level = _level,
                GroupId = _groupId,
                CompletedChapters = _completedChapters,
                CompletedTokens = _completedTokens,
                CompletedSequenceNumbers = _completedSequenceNumbers
            };
        }
    }
}