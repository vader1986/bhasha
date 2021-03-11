using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BhashaController
    {
        private readonly IDatabase _database;

        public UserController(IDatabase database)
        {
            _database = database;
        }

        // Authorize User (?)
        [HttpPost("create")]
        public async Task<ActionResult<User>> Create(string userName, string email)
        {
            return await _database.CreateUser(new User(default, userName, email));
        }

        // Authorize User
        [HttpPatch("update")]
        public async Task<IActionResult> Update(string userName, string email)
        {
            await _database.UpdateUser(new User(UserId, userName, email));
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete()
        {
            var profiles = await _database.GetProfiles(UserId);

            var deleteChapterStats =
                Task.WhenAll(profiles
                    .Select(x => x.Id)
                    .Select(_database.DeleteChapterStatsForProfile));

            var deleteProfiles = (Task)_database.DeleteProfiles(UserId);
            var deleteUser = (Task)_database.DeleteUser(UserId);

            await Task.WhenAll(deleteProfiles, deleteChapterStats, deleteUser);

            return Ok();
        }
    }
}
