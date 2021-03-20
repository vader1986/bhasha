using System.Linq;

namespace Bhasha.Common.Extensions
{
    public static class ChapterStatsExtensions
    {
        public static ChapterStats WithCompleted(this ChapterStats stats)
        {
            return new ChapterStats(
                stats.Id,
                stats.ProfileId,
                stats.ChapterId,
                true,
                stats.Tips,
                stats.Submits,
                stats.Failures);
        }

        public static ChapterStats WithFailure(this ChapterStats stats, int pageIndex)
        {
            stats.Submits.Inc(pageIndex);
            stats.Failures.Inc(pageIndex);
            return stats;
        }

        public static ChapterStats WithSuccess(this ChapterStats stats, int pageIndex)
        {
            stats.Submits.Inc(pageIndex);
            return stats;
        }

        public static ChapterStats WithTip(this ChapterStats stats, int pageIndex)
        {
            stats.Tips.Inc(pageIndex);
            return stats;
        }
    }
}
