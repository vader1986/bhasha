using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Services
{
    public interface IUpdateStats
    {
        Task FromEvaluation(Evaluation evaluation, Profile profile, GenericChapter chapter);
    }

    public class StatsUpdater : IUpdateStats
    {
        private readonly IDatabase _database;
        private readonly IStore<ChapterStats> _stats;
        private readonly IStore<Profile> _profiles;

        public StatsUpdater(IDatabase database, IStore<ChapterStats> stats, IStore<Profile> profiles)
        {
            _database = database;
            _stats = stats;
            _profiles = profiles;
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
    }
}
