using System.Threading.Tasks;
using Bhasha.Common.Importers;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentController : BaseController
    {
        private readonly ChapterDtoImporter _importer;

        public ContentController(ChapterDtoImporter importer)
        {
            _importer = importer;
        }

        // Authorize Author
        [HttpPost("add/chapter")]
        public async Task AddToken([FromBody]ChapterDto chapter)
        {
            await _importer.Import(chapter);
        }
    }
}
