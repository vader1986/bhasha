using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Student.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IDatabase _database;
        private readonly IStore<DbStats> _stats;
        private readonly IStore<DbUserProfile> _profiles;

        public UserController(IDatabase database, IStore<DbStats> stats, IStore<DbUserProfile> profiles)
        {
            _database = database;
            _stats = stats;
            _profiles = profiles;
        }

        [HttpDelete("delete")]
        public async Task Delete()
        {
            var profiles = await _database.QueryProfiles(UserId);

            var chapterStats = await Task.WhenAll(profiles
                .Select(x => _database.QueryStats(x.Id)));

            await Task.WhenAll(chapterStats
                .SelectMany(x => x)
                .Select(x => _stats.Remove(x.Id))
                .Concat(profiles.Select(x => _profiles.Remove(x.Id))));
        }
    }
}
