using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Importers;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Author.Api.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentController : Controller
    {
        private readonly ChapterImporter _importer;

        public ContentController(ChapterImporter importer)
        {
            _importer = importer;
        }

        [HttpPost("add/chapter")]
        public Task<DbTranslatedChapter> AddChapter([FromBody] DbTranslatedChapter chapter)
        {
            return _importer.Import(chapter);
        }

        [HttpPost("add/expression")]
        public Task<DbExpression> AddExpression([FromBody] DbExpression expression)
        {
            return _importer.Import(expression);
        }

        [HttpPost("add/word")]
        public Task<DbWord> AddWord([FromBody] DbWord word)
        {
            return _importer.Import(word);
        }
    }
}
