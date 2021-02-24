using System.Threading.Tasks;
using Bhasha.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Api.Controllers
{
    [ApiController]
    [Route("api/chapter")]
    public class ChapterController : Controller
    {
        [HttpGet("next/{userId}")]
        public async Task<ActionResult<Chapter>> GetNext(string userId)
        {
            return Ok(null);
        }
    }
}
