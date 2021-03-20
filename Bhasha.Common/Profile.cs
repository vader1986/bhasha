using System;

namespace Bhasha.Common
{
    public class Profile : IEntity
    {
        /// <summary>
        /// Unique identifier of this user profile.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Reference to the user the profile is linked to.
        /// </summary>
        public string UserId { get; }

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

        /// <summary>
        /// Total number of completed chapters.
        /// </summary>
        public int CompletedChapters { get; }

        public Profile(Guid id, string userId, Language from, Language to, int level, int completedChapters)
        {
            Id = id;
            UserId = userId;
            From = from;
            To = to;
            Level = level;
            CompletedChapters = completedChapters;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(UserId)}: {UserId}, {nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(Level)}: {Level}, {nameof(CompletedChapters)}: {CompletedChapters}";
        }
    }
}
