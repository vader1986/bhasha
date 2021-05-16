using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Database;

namespace Bhasha.Common.Services
{
    public interface IChaptersLookup
    {
        Task<ChapterEnvelope[]> GetChapters(Profile profile, int requestedLevel = int.MaxValue);
    }

    public class ChaptersLookup : IChaptersLookup
    {
        private readonly IDatabase _database;
        private readonly IStore<DbTranslatedChapter> _chapters;
        private readonly IStore<DbStats> _stats;
        private readonly IConvert<DbTranslatedChapter, Chapter> _convertChapters;
        private readonly IConvert<DbStats, Stats> _convertStats;

        public ChaptersLookup(IDatabase database, IStore<DbTranslatedChapter> chapters, IStore<DbStats> stats, IConvert<DbTranslatedChapter, Chapter> convertChapters, IConvert<DbStats, Stats> convertStats)
        {
            _database = database;
            _chapters = chapters;
            _stats = stats;
            _convertChapters = convertChapters;
            _convertStats = convertStats;
        }

        private async Task<Stats> LoadStats(Guid chapterId, Guid profileId, int pages)
        {
            var stats =
                await _database.QueryStats(chapterId, profileId) ??
                await _stats.Add(DbStats.Create(profileId, chapterId, pages));

            return _convertStats.Convert(stats);
        }

        private async Task<ChapterEnvelope?> LoadChapterEnvelope(DbChapter dbChapter, Profile profile)
        {
            var chapter = await _chapters.Get(dbChapter.Id);
            if (chapter == null)
            {
                return default;
            }

            var stats = await LoadStats(chapter.Id, profile.Id, chapter.Pages!.Length);

            return new ChapterEnvelope(_convertChapters.Convert(chapter), stats);
        }

        public async Task<ChapterEnvelope[]> GetChapters(Profile profile, int requestedLevel)
        {
            var appliedLevel = Math.Min(profile.Level, requestedLevel);
            var dbChapters = await _database.QueryChapters(appliedLevel);

            var chapters = await Task.WhenAll(
                dbChapters.Select(async chapter => await LoadChapterEnvelope(chapter, profile)));

            return chapters.Where(x => x != null).ToArray()!;
        }
    }
}
