using System;

namespace Bhasha.Common
{
    public class Profile : IEquatable<Profile?>
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
        public Language Native { get; }

        /// <summary>
        /// Target language to learn.
        /// </summary>
        public Language Target { get; }

        /// <summary>
        /// Language level the user has reached within this profile.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Total number of completed chapters.
        /// </summary>
        public int CompletedChapters { get; private set; }

        public Profile(Guid id, string userId, Language native, Language target, int level, int completedChapters)
        {
            if (native == target)
            {
                throw new ArgumentException(nameof(target));
            }

            Id = id;
            UserId = userId;
            Native = native;
            Target = target;
            Level = level;
            CompletedChapters = completedChapters;
        }

        public void CompleteLevel()
        {
            Level++;
        }

        public void CompleteChapter()
        {
            CompletedChapters++;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Profile);
        }

        public bool Equals(Profile? other)
        {
            return other != null && Id.Equals(other.Id) && UserId == other.UserId && Native == other.Native && Target == other.Target && Level == other.Level && CompletedChapters == other.CompletedChapters;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, UserId, Native, Target, Level, CompletedChapters);
        }

        public static bool operator ==(Profile? left, Profile? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Profile? left, Profile? right)
        {
            return !(left == right);
        }
    }
}
