using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentController : BaseController
    {
        private readonly IStore<Token> _tokens;
        private readonly IStore<GenericChapter> _chapters;
        private readonly IStore<Translation> _translations;

        public ContentController(IStore<Token> tokens, IStore<GenericChapter> chapters, IStore<Translation> translations)
        {
            _tokens = tokens;
            _chapters = chapters;
            _translations = translations;
        }

        // Authorize Author
        [HttpPost("add/token")]
        public async Task<Token> AddToken([FromBody]Token token)
        {
            return await _tokens.Add(token);
        }

        // Authorize Author
        [HttpPost("add/chapter")]
        public async Task<GenericChapter> AddChapter([FromBody]GenericChapter chapter)
        {
            return await _chapters.Add(chapter);
        }

        // Authorize Author
        [HttpPost("add/translation")]
        public async Task<Translation> AddTranslation([FromBody]Translation translation)
        {
            return await _translations.Add(translation);
        }
    }
}
