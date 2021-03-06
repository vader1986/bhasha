using System.Collections;

namespace Bhasha.Common
{
    public class ChapterStats
    {
        /// <summary>
        /// Reference to the profile these stats are linked to.
        /// </summary>
        public int ProfileId { get; }

        /// <summary>
        /// Reference to the chapter these stats are linked to.
        /// </summary>
        public int ChapterId { get; }

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

        public ChapterStats(int profileId, int chapterId, bool completed, byte[] tips, byte[] submits, byte[] failures)
        {
            ProfileId = profileId;
            ChapterId = chapterId;
            Completed = completed;
            Tips = tips;
            Submits = submits;
            Failures = failures;
        }
    }
}
