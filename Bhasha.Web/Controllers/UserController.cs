using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BhashaController
    {
        private readonly IDatabase _database;
        private readonly IStore<User> _users;
        private readonly IStore<ChapterStats> _stats;
        private readonly IStore<Profile> _profiles;

        public UserController(IDatabase database, IStore<User> users, IStore<ChapterStats> stats, IStore<Profile> profiles)
        {
            _database = database;
            _users = users;
            _stats = stats;
            _profiles = profiles;
        }

        // Authorize User (?)
        [HttpPost("create")]
        public async Task<User> Create(string userName, string email)
        {
            return await _users.Add(new User(default, userName, email));
        }

        // Authorize User
        [HttpPatch("update")]
        public async Task Update(string userName, string email)
        {
            await _users.Replace(new User(UserId, userName, email));
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
                .Select(_profiles.Remove))
                .Append(_users.Remove(await _users.Get(UserId))));
        }
    }
}
