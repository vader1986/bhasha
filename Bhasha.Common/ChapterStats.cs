using System;

namespace Bhasha.Common
{
    public class ChapterStats : IEntity
    {
        /// <summary>
        /// Unique identifier for the stats.
        /// </summary>
        public Guid Id { get; }

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
        public int Tips { get; }

        /// <summary>
        /// Number of submits for each page.
        /// </summary>
        public byte[] Submits { get; }

        /// <summary>
        /// Number of failed submits for each page.
        /// </summary>
        public byte[] Failures { get; }

        public ChapterStats(Guid id, Guid profileId, Guid chapterId, bool completed, int tips, byte[] submits, byte[] failures)
        {
            Id = id;
            ProfileId = profileId;
            ChapterId = chapterId;
            Completed = completed;
            Tips = tips;
            Submits = submits;
            Failures = failures;
        }

        public static ChapterStats Create(Guid profileId, GenericChapter genericChapter)
        {
            var pages = genericChapter.Pages.Length;
            return new ChapterStats(default, profileId, genericChapter.Id, false, 0, new byte[pages], new byte[pages]);
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(ProfileId)}: {ProfileId}, {nameof(ChapterId)}: {ChapterId}, {nameof(Completed)}: {Completed}, {nameof(Tips)}: {Tips}, {nameof(Submits)}: {string.Join("/", Submits)}, {nameof(Failures)}: {string.Join("/", Failures)}";
        }
    }
}
