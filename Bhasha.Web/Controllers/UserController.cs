using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IDatabase _database;
        private readonly IStore<ChapterStats> _stats;
        private readonly IStore<Profile> _profiles;

        public UserController(IDatabase database, IStore<ChapterStats> stats, IStore<Profile> profiles)
        {
            _database = database;
            _stats = stats;
            _profiles = profiles;
        }

        [HttpDelete("delete")]
        public async Task Delete()
        {
            var profiles = await _database
                .QueryProfilesByUserId(UserId);

            var chapterStats = await Task.WhenAll(profiles
                .Select(x => x.Id)
                .Select(_database.QueryStatsByProfileId));

            await Task.WhenAll(chapterStats
                .SelectMany(x => x)
                .Select(_stats.Remove)
                .Concat(profiles
                .Select(_profiles.Remove)));
        }
    }
}
