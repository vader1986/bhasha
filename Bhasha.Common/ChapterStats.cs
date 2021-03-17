using System;

namespace Bhasha.Common
{
    public class ChapterStats
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

        public ChapterStats(Guid profileId, Guid chapterId, bool completed, byte[] tips, byte[] submits, byte[] failures)
        {
            ProfileId = profileId;
            ChapterId = chapterId;
            Completed = completed;
            Tips = tips;
            Submits = submits;
            Failures = failures;
        }

        public static ChapterStats Empty(Guid profileId, Guid chapterId, int pages)
        {
            return new ChapterStats(profileId, chapterId, false, new byte[pages], new byte[pages], new byte[pages]);
        }

        public override string ToString()
        {
            return $"{nameof(ProfileId)}: {ProfileId}, {nameof(ChapterId)}: {ChapterId}, {nameof(Completed)}: {Completed}, {nameof(Tips)}: {string.Join("/", Tips)}, {nameof(Submits)}: {string.Join("/", Submits)}, {nameof(Failures)}: {string.Join("/", Failures)}";
        }
    }
}
