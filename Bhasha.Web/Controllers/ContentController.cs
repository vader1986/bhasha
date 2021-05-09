using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Importers;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentController : BaseController
    {
        private readonly ChapterImporter _importer;

        public ContentController(ChapterImporter importer)
        {
            _importer = importer;
        }

        // Authorize Author
        [HttpPost("add/chapter")]
        public Task<DbTranslatedChapter> AddToken([FromBody] DbTranslatedChapter chapter)
        {
            return _importer.Import(chapter);
        }
    }
}
