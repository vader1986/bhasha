using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Arguments;

namespace Bhasha.Common.Services
{
    public interface IAssembleChapters
    {
        Task<Chapter?> Assemble(GenericChapter chapter, Profile profile);
    }

    public class ChapterAssembly : IAssembleChapters
    {
        private readonly IDatabase _database;
        private readonly IStore<Token> _tokens;
        private readonly IArgumentAssemblyProvider _arguments;

        public ChapterAssembly(IDatabase database, IStore<Token> tokens, IArgumentAssemblyProvider arguments)
        {
            _database = database;
            _tokens = tokens;
            _arguments = arguments;
        }

        public async Task<Chapter?> Assemble(GenericChapter chapter, Profile profile)
        {
            var token = await _tokens.Get(chapter.NameId);
            if (token == null)
            {
                return null;
            }

            var name = await _database.QueryTranslationByTokenId(chapter.NameId, profile.From);
            if (name == null)
            {
                return null;
            }

            var description = await _database.QueryTranslationByTokenId(chapter.DescriptionId, profile.From);
            if (description == null)
            {
                return null;
            }

            var translations = await Task
                .WhenAll(chapter
                .Pages
                .Select(p => _database
                .QueryTranslationByTokenId(p.TokenId, profile.From)));

            if (translations == null)
            {
                return null;
            }

            async Task<Page?> PageFor(GenericPage genericPage)
            {
                var token = await _tokens
                    .Get(genericPage.TokenId);

                if (token == null)
                {
                    return null;
                }

                var translation = translations
                    .FirstOrDefault(x => x != null &&
                                         x.TokenId == genericPage.TokenId);

                if (translation == null)
                {
                    return null;
                }

                var arguments = _arguments
                    .GetAssembly(genericPage.PageType)
                    .Assemble(translations, genericPage.TokenId);

                return new Page(genericPage.PageType, token, translation, arguments);
            }

            var pages = await Task.WhenAll(chapter.Pages.Select(PageFor));
            if (pages.Any(x => x == null))
            {
                return null;
            }

            return new Chapter(
                chapter.Id,
                chapter.Level,
                name.Native,
                description.Native,
                pages,
                token?.PictureId);
        }
    }
}
