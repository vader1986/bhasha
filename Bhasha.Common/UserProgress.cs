namespace Bhasha.Common
{
    public class UserProgress
    {
        public EntityId UserId { get; }
        public Language From { get; }
        public Language To { get; }
        public LanguageLevel Level { get; }
        public Category[] Finished { get; }

        public UserProgress(EntityId userId, Language from, Language to, LanguageLevel level, Category[] finished)
        {
            UserId = userId;
            From = from;
            To = to;
            Level = level;
            Finished = finished;
        }
    }
}
