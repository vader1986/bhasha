using System;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Services
{
    public class UpdateStatsOnTipRequest : IUpdateStatsOnTipRequest
    {
        private readonly IDatabase _database;
        private readonly IStore<DbStats> _stats;
        private readonly IStore<DbChapter> _chapters;

        public UpdateStatsOnTipRequest(IDatabase database, IStore<DbStats> stats, IStore<DbChapter> chapters)
        {
            _database = database;
            _stats = stats;
            _chapters = chapters;
        }

        public async Task Update(Profile profile, Guid chapterId, int pageIndex)
        {
            var stats = await _database.QueryStats(chapterId, profile.Id);

            if (stats == default)
            {
                var chapter = await _chapters.Get(chapterId);

                if (chapter != null)
                {
                    var pages = chapter.Pages?.Length ?? 0;
                    stats = await _stats.Add(DbStats.Create(profile.Id, chapter.Id, pages));
                }
                else
                {
                    throw new ObjectNotFoundException(typeof(DbChapter), chapterId);
                }
            }

            await _stats.Replace(stats.WithTip(pageIndex));
        }
    }
}
