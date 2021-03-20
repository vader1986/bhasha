using System.Threading.Tasks;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Services
{
    public interface IUpdateStatsForTip
    {
        Task UpdateStats(Tip tip, Profile profile);
    }

    public class TipStatsUpdater : IUpdateStatsForTip
    {
        private readonly IDatabase _database;
        private readonly IStore<ChapterStats> _stats;
        private readonly IStore<GenericChapter> _chapters;

        public TipStatsUpdater(IDatabase database, IStore<ChapterStats> stats, IStore<GenericChapter> chapters)
        {
            _database = database;
            _stats = stats;
            _chapters = chapters;
        }

        public async Task UpdateStats(Tip tip, Profile profile)
        {
            var stats =
                await _database.QueryStatsByChapterAndProfileId(tip.ChapterId, profile.Id);

            if (stats == default)
            {
                var chapter = await _chapters.Get(tip.ChapterId);
                stats = await _stats.Add(ChapterStats.Create(profile.Id, chapter));
            }

            await _stats.Replace(stats.WithTip(tip.PageIndex));
        }
    }
}
