using System.Collections.Generic;

namespace Bhasha.Common
{
    public class UserProgress
    {
        public EntityId UserId { get; }
        public Language From { get; }
        public Language To { get; }
        public LanguageLevel Level { get; }
        public UserStats Stats { get; }

        public UserProgress(EntityId userId, Language from, Language to, LanguageLevel level, UserStats stats)
        {
            UserId = userId;
            From = from;
            To = to;
            Level = level;
            Stats = stats;
        }
    }
}
