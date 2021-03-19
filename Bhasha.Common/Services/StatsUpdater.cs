using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Services
{
    public interface IUpdateStats
    {
        Task FromEvaluation(Evaluation evaluation, Profile profile, GenericChapter chapter);
        Task FromTip(Tip tip, Profile profile);
    }

    public class StatsUpdater : IUpdateStats
    {
        private readonly IDatabase _database;
        private readonly IStore<ChapterStats> _stats;
        private readonly IStore<Profile> _profiles;
        private readonly IStore<GenericChapter> _chapters;

        public StatsUpdater(IDatabase database, IStore<ChapterStats> stats, IStore<Profile> profiles, IStore<GenericChapter> chapters)
        {
            _database = database;
            _stats = stats;
            _profiles = profiles;
            _chapters = chapters;
        }

        private async Task UpdateProfile(Profile profile)
        {
            var completedChapters = profile.CompletedChapters + 1;

            await _profiles.Replace(new Profile(
                profile.Id,
                profile.UserId,
                profile.From,
                profile.To,
                completedChapters / 5 + 1,
                completedChapters));
        }

        private bool JustCompletedChapter(ChapterStats stats)
        {
            bool PageCompleted(byte submits, int index)
            {
                return
                    submits > stats.Failures[index] ||
                    submits == byte.MaxValue;
            }

            return
                stats
                    .Completed == false &&
                stats
                    .Submits
                    .Select(PageCompleted)
                    .All(x => x);
        }

        public async Task FromEvaluation(Evaluation evaluation, Profile profile, GenericChapter chapter)
        {
            var stats =
                await _database.QueryStatsByChapterAndProfileId(chapter.Id, profile.Id) ??
                await _stats.Add(ChapterStats.Create(profile.Id, chapter));

            var pageIndex = evaluation.Submit.PageIndex;

            if (evaluation.Result == Result.Correct)
            {
                var updatedStats = stats.WithSuccess(pageIndex);

                if (JustCompletedChapter(updatedStats))
                {
                    await UpdateProfile(profile);
                    await _stats.Replace(updatedStats.WithCompleted());
                }
                else
                {
                    await _stats.Replace(updatedStats);
                }
            }
            else
            {
                await _stats.Replace(stats.WithFailure(pageIndex));
            }
        }

        public async Task FromTip(Tip tip, Profile profile)
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
