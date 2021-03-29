using System.Threading.Tasks;
using Bhasha.Common;
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
        public Task<GenericChapter> AddToken([FromBody]ChapterDto chapter)
        {
            return _importer.Import(chapter);
        }
    }
}
