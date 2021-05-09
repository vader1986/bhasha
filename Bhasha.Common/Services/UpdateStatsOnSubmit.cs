using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Database;

namespace Bhasha.Common.Services
{
    public class UpdateStatsOnSubmit : IUpdateStatsOnSubmit
    {
        private readonly IDatabase _database;
        private readonly IStore<DbStats> _stats;
        private readonly IStore<DbUserProfile> _profiles;

        public UpdateStatsOnSubmit(IDatabase database, IStore<DbStats> stats, IStore<DbUserProfile> profiles)
        {
            _database = database;
            _stats = stats;
            _profiles = profiles;
        }

        private async Task UpdateProfile(Profile profile)
        {
            profile.CompleteChapter();

            var chapters = await _database.QueryChapters(profile.Level);

            if (profile.CompletedChapters == chapters.Count())
            {
                profile.CompleteLevel();
            }

            await _profiles.Replace(new DbUserProfile {
                Id = profile.Id,
                UserId = profile.UserId,
                Languages = new DbProfile {
                    Native = profile.Native,
                    Target = profile.Target
                },
                Level = profile.Level,
                CompletedChapters = profile.CompletedChapters
            });
        }

        public async Task<Evaluation> Update(Evaluation evaluation, int pages)
        {
            var profileId = evaluation.Profile.Id;
            var chapterId = evaluation.Submit.ChapterId;
            var pageIndex = evaluation.Submit.PageIndex;

            var stats =
                await _database.QueryStats(chapterId, profileId) ??
                await _stats.Add(DbStats.Create(profileId, chapterId, pages));

            if (evaluation.Result == Result.Correct)
            {
                var completedBefore = stats.Completed;

                stats = stats
                    .WithSubmit(pageIndex)
                    .WithUpdatedCompleted();

                await _stats.Replace(stats);

                if (completedBefore != stats.Completed)
                {
                    await UpdateProfile(evaluation.Profile);
                }
            }
            else
            {
                await _stats.Replace(stats.WithFailure(pageIndex));
            }

            return evaluation;
        }
    }
}
