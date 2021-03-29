using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Services;

namespace Bhasha.Common.Importers
{
    public class ChapterDtoImporter
    {
        private readonly IDatabase _database;
        private readonly IStore<Token> _tokens;
        private readonly IStore<Translation> _translations;
        private readonly IStore<GenericChapter> _chapters;

        public ChapterDtoImporter(IDatabase database, IStore<Token> tokens, IStore<Translation> translations, IStore<GenericChapter> chapters)
        {
            _database = database;
            _tokens = tokens;
            _translations = translations;
            _chapters = chapters;
        }

        private async Task<Token> ImportToken(ChapterDto.TokenDto dto, int level)
        {
            var token = await _database.QueryTokenByLabel(dto.Label);
            if (token != null)
            {
                return token;
            }

            return await _tokens.Add(new Token(
                    dto.Label,
                    level,
                    Enum.Parse<CEFR>(dto.Cefr),
                    Enum.Parse<TokenType>(dto.TokenType),
                    dto.Categories));
        }

        private async Task ImportTranslation(string language, ChapterDto.TranslationDto dto, Token token)
        {
            var translation = await _database.QueryTranslationByTokenId(token.Id, language);
            if (translation == null)
            {
                await _translations.Add(new Translation(token.Id, language, dto.Native, dto.Spoken));
            }
        }

        private async Task<GenericPage> ImportPage(string from, string to, ChapterDto.PageDto page, int level)
        {
            var token = await ImportToken(page.Token, level);
            await ImportTranslation(from, page.From, token);
            await ImportTranslation(to, page.To, token);

            return new GenericPage(token.Id, PageType.OneOutOfFour);
        }

        public async Task<GenericChapter> Import(ChapterDto dto)
        {
            var nameToken = await ImportToken(dto.Name.Token, dto.Level);
            await ImportTranslation(dto.From, dto.Name.Native, nameToken);

            var descToken = await ImportToken(dto.Description.Token, dto.Level);
            await ImportTranslation(dto.From, dto.Description.Native, descToken);

            var pages = await Task.WhenAll(dto.Pages.Select(x => ImportPage(dto.From, dto.To, x, dto.Level)));

            var chapter = new GenericChapter(dto.Level, nameToken.Id, descToken.Id, pages);

            var existingChapters = await _database.QueryChaptersByLevel(dto.Level);
            var existingChapter = existingChapters.FirstOrDefault(x => x.Equals(chapter));

            if (existingChapter != null)
            {
                return existingChapter;
            }

            return await _chapters.Add(chapter);
        }
    }
}
