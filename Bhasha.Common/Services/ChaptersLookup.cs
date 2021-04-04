using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bhasha.Common.Services
{
    public interface IChaptersLookup
    {
        Task<Chapter[]> GetChapters(Profile profile, int requestedLevel = int.MaxValue);
    }

    public class ChaptersLookup : IChaptersLookup
    {
        private readonly IDatabase _database;
        private readonly IAssembleChapters _chapters;

        public ChaptersLookup(IDatabase database, IAssembleChapters chapters)
        {
            _database = database;
            _chapters = chapters;
        }

        public async Task<Chapter[]> GetChapters(Profile profile, int requestedLevel)
        {
            var appliedLevel = Math.Min(profile.Level, requestedLevel);
            var chapters = await _database.QueryChaptersByLevel(appliedLevel);
            var result = await Task.WhenAll(chapters.Select(async x => await _chapters.Assemble(x, profile)));

            return result.Where(x => x != null).ToArray();
        }
    }
}
