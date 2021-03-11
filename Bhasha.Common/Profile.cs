using System;

namespace Bhasha.Common
{
    public class Profile
    {
        /// <summary>
        /// Unique identifier of this user profile.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Reference to the user the profile is linked to.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Native language for this profile.
        /// </summary>
        public Language From { get; }

        /// <summary>
        /// Language to learn.
        /// </summary>
        public Language To { get; }

        /// <summary>
        /// Language level the user has reached within this profile.
        /// </summary>
        public int Level { get; }

        public Profile(Guid id, Guid userId, Language from, Language to, int level)
        {
            Id = id;
            UserId = userId;
            From = from;
            To = to;
            Level = level;
        }
    }
}
