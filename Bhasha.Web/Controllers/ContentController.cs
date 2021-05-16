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
        public Task<DbTranslatedChapter> AddChapter([FromBody] DbTranslatedChapter chapter)
        {
            return _importer.Import(chapter);
        }

        // Authorize Author
        [HttpPost("add/expression")]
        public Task<DbExpression> AddExpression([FromBody] DbExpression expression)
        {
            return _importer.Import(expression);
        }

        // Authorize Author
        [HttpPost("add/word")]
        public Task<DbWord> AddWord([FromBody] DbWord word)
        {
            return _importer.Import(word);
        }
    }
}
