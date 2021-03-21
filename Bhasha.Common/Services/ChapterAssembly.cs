using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Arguments;
using Bhasha.Common.Exceptions;

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
        private readonly ArgumentAssemblyProvider _arguments;

        public ChapterAssembly(IDatabase database, IStore<Token> tokens, IStore<GenericChapter> chapters, ArgumentAssemblyProvider arguments)
        {
            _database = database;
            _tokens = tokens;
            _chapters = chapters;
            _arguments = arguments;
        }

        public async Task<Chapter> Assemble(Guid chapterId, Profile profile)
        {
            var chapter = await _chapters.Get(chapterId);

            if (chapter == null)
            {
                throw new ObjectNotFoundException(typeof(GenericChapter), chapterId);
            }

            var translations = await Task
                .WhenAll(chapter
                .Pages
                .Select(p => _database
                .QueryTranslationByTokenId(p.TokenId, profile.From)));

            async Task<Page> PageFor(GenericPage genericPage)
            {
                var token = await _tokens
                    .Get(genericPage.TokenId);

                if (token == null)
                {
                    throw new ObjectNotFoundException(typeof(Token), genericPage.TokenId);
                }

                var translation = translations
                    .First(x => x.TokenId == genericPage.TokenId);

                var arguments = _arguments
                    .Invoke(genericPage.PageType)
                    .Assemble(translations, genericPage.TokenId);

                return new Page(genericPage.PageType, token, translation, arguments);
            }

            var pages = await Task.WhenAll(chapter.Pages.Select(PageFor));

            return new Chapter(
                chapter.Id,
                chapter.Level,
                chapter.Name,
                chapter.Description,
                pages,
                chapter.PictureId);
        }
    }
}
