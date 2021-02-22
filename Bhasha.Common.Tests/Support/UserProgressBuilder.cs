namespace Bhasha.Common.Tests.Support
{
    public class UserProgressBuilder
    {
        private EntityId _userId = new EntityId("user-123");
        private Language _from = Languages.English;
        private Language _to = Languages.Bengoli;
        private LanguageLevel _level = LanguageLevel.B2;
        private Category[] _finished = new[] { new Category("vehicle") };

        public static UserProgressBuilder Default => new UserProgressBuilder();
        public static UserProgress Create() => Default.Build();

        public UserProgressBuilder WithUserId(EntityId userId)
        {
            _userId = userId;
            return this;
        }

        public UserProgressBuilder WithFrom(Language from)
        {
            _from = from;
            return this;
        }

        public UserProgressBuilder WithTo(Language to)
        {
            _to = to;
            return this;
        }

        public UserProgressBuilder WithLevel(LanguageLevel level)
        {
            _level = level;
            return this;
        }

        public UserProgressBuilder WithFinished(Category[] finished)
        {
            _finished = finished;
            return this;
        }

        public UserProgress Build()
        {
            return new UserProgress(_userId, _from, _to, _level, _finished);
        }
    }
}
