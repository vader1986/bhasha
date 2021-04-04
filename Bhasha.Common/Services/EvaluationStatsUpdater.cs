using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Services
{
    public interface IUpdateStatsForEvaluation
    {
        Task<Profile> UpdateStats(Result result, int pageIndex, Profile profile, GenericChapter chapter);
    }

    public class EvaluationStatsUpdater : IUpdateStatsForEvaluation
    {
        private readonly IDatabase _database;
        private readonly IStore<ChapterStats> _stats;
        private readonly IStore<Profile> _profiles;

        public EvaluationStatsUpdater(IDatabase database, IStore<ChapterStats> stats, IStore<Profile> profiles)
        {
            _database = database;
            _stats = stats;
            _profiles = profiles;
        }

        private async Task<Profile> UpdateProfile(Profile profile)
        {
            var completedChapters = profile.CompletedChapters + 1;

            var chapters = await _database.QueryChaptersByLevel(profile.Level);
            var stats = await Task.WhenAll(chapters.Select(chapter => _database.QueryStatsByChapterAndProfileId(chapter.Id, profile.Id)));
            var level = stats.Any(x => x == null || !x.Completed) ? profile.Level : profile.Level + 1;

            var updatedProfile = new Profile(
                profile.Id,
                profile.UserId,
                profile.From,
                profile.To,
                level,
                completedChapters);

            await _profiles.Replace(updatedProfile);

            return updatedProfile;
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

        public async Task<Profile> UpdateStats(Result result, int pageIndex, Profile profile, GenericChapter chapter)
        {
            var stats =
                await _database.QueryStatsByChapterAndProfileId(chapter.Id, profile.Id) ??
                await _stats.Add(ChapterStats.Create(profile.Id, chapter));

            if (stats.Completed)
            {
                return profile;
            }

            if (result == Result.Correct)
            {
                var updatedStats = stats.WithSuccess(pageIndex);

                if (JustCompletedChapter(updatedStats))
                {
                    await _stats.Replace(updatedStats.WithCompleted());
                    return await UpdateProfile(profile);
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

            return profile;
        }
    }
}
