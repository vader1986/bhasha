using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Arguments;

namespace Bhasha.Common.Services
{
    public interface IAssembleChapters
    {
        Task<Chapter> Assemble(Guid chapterId, Profile profile);
    }

    public class ChapterAssembly : IAssembleChapters
    {
        private readonly IDatabase _database;
        private readonly IStore<Token> _tokens;
        private readonly IStore<GenericChapter> _chapters;
        private readonly IAssembleArguments[] _arguments;

        public ChapterAssembly(IDatabase database, IStore<Token> tokens, IStore<GenericChapter> chapters, IAssembleArguments[] arguments)
        {
            _database = database;
            _tokens = tokens;
            _chapters = chapters;
            _arguments = arguments;
        }

        public async Task<Chapter> Assemble(Guid chapterId, Profile profile)
        {
            var genericChapter = await _chapters.Get(chapterId);
            var translations = await Task
                .WhenAll(genericChapter
                .Pages
                .Select(p => _database
                .QueryTranslationByTokenId(p.TokenId, profile.From)));

            async Task<Page> PageFor(GenericPage genericPage)
            {
                var token = await _tokens
                    .Get(genericPage.TokenId);

                var translation = translations
                    .First(x => x.TokenId == genericPage.TokenId);

                var arguments = _arguments
                    .First(x => x.Supports(genericPage.PageType))
                    .Assemble(translations, genericPage.TokenId);

                return new Page(genericPage.PageType, token, translation, arguments);
            }

            var pages = await Task.WhenAll(genericChapter.Pages.Select(PageFor));

            return new Chapter(
                genericChapter.Id,
                genericChapter.Level,
                genericChapter.Name,
                genericChapter.Description,
                pages,
                genericChapter.PictureId);
        }
    }
}
