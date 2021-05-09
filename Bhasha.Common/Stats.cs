using System;
using System.Linq;

namespace Bhasha.Common
{
    public class Stats : IEquatable<Stats?>
    {
        /// <summary>
        /// Reference to the profile these stats are linked to.
        /// </summary>
        public Guid ProfileId { get; }

        /// <summary>
        /// Reference to the chapter these stats are linked to.
        /// </summary>
        public Guid ChapterId { get; }

        /// <summary>
        /// Whether or not the chapter has been completed.
        /// </summary>
        public bool Completed { get; }

        /// <summary>
        /// Tips used for each page of the chapter.
        /// </summary>
        public byte[] Tips { get; }

        /// <summary>
        /// Number of submits for each page.
        /// </summary>
        public byte[] Submits { get; }

        /// <summary>
        /// Number of failed submits for each page.
        /// </summary>
        public byte[] Failures { get; }

        public Stats(Guid profileId, Guid chapterId, bool completed, byte[] tips, byte[] submits, byte[] failures)
        {
            ProfileId = profileId;
            ChapterId = chapterId;
            Completed = completed;
            Tips = tips;
            Submits = submits;
            Failures = failures;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Stats);
        }

        public bool Equals(Stats? other)
        {
            return other != null && ProfileId.Equals(other.ProfileId) && ChapterId.Equals(other.ChapterId) && Completed == other.Completed && Tips.SequenceEqual(other.Tips) && Submits.SequenceEqual(other.Submits) && Failures.SequenceEqual(other.Failures);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProfileId, ChapterId, Completed, Tips, Submits, Failures);
        }

        public static bool operator ==(Stats? left, Stats? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Stats? left, Stats? right)
        {
            return !(left == right);
        }
    }
}
