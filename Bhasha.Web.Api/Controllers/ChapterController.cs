using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Aggregation;
using Bhasha.Common.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Api.Controllers
{
    [ApiController]
    [Route("api/chapter")]
    public class ChapterController : Controller
    {
        private readonly IDatabase _database;
        private readonly ILoadChapter _chapters;

        public ChapterController(IDatabase database, ILoadChapter chapters)
        {
            _database = database;
            _chapters = chapters;
        }

        [HttpGet("next/{userId}")]
        public async Task<ActionResult<Chapter>> GetNext(string userId)
        {
            var users = await _database.Query(new UserProgressQueryById(1, new EntityId(userId)));
            var user = users.Single();

            var chapter = await _chapters.NextChapter(user);

            return Ok(chapter);
        }
    }
}
