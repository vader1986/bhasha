namespace Bhasha.Common
{
    public class UserProgress
    {
        public EntityId UserId { get; }
        public Language From { get; }
        public Language To { get; }
        public UserStats Stats { get; }

        public UserProgress(EntityId userId, Language from, Language to, UserStats stats)
        {
            UserId = userId;
            From = from;
            To = to;
            Stats = stats;
        }
    }
}
