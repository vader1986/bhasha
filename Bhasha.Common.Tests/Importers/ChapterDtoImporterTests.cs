using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;
using Bhasha.Common.Importers;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Importers
{
    [TestFixture]
    public class ChapterDtoImporterTests
    {
        private IDatabase _database;
        private IStore<Token> _tokens;
        private IStore<Translation> _translations;
        private IStore<GenericChapter> _chapters;
        private ChapterDtoImporter _importer;

        private readonly Token AToken = TokenBuilder.Default.Build();
        private readonly Translation ATranslation = TranslationBuilder.Default.Build();
        private readonly ChapterDto Chapter = new ChapterDto
        {
            From = Language.English,
            To = Language.Bengali,
            Level = 1,
            Description = new ChapterDto.ExpressionDto {
                Native = new ChapterDto.TranslationDto {
                    Native = "Native description",
                    Spoken = "native description"
                },
                Token = new ChapterDto.TokenDto {
                    Cefr = "A2",
                    Label = "native description",
                    TokenType = "Expression",
                    Categories = new[] { "description" }
                }
            },
            Name = new ChapterDto.ExpressionDto
            {
                Native = new ChapterDto.TranslationDto
                {
                    Native = "Name",
                    Spoken = "name"
                },
                Token = new ChapterDto.TokenDto
                {
                    Cefr = "A1",
                    Label = "name",
                    TokenType = "Noun",
                    Categories = new string[0]
                }
            },
            Pages = new ChapterDto.PageDto[] {
                new ChapterDto.PageDto {
                    From = new ChapterDto.TranslationDto {
                        Native = "cat",
                        Spoken = "kat"
                    },
                    To = new ChapterDto.TranslationDto {
                        Native = "বিড়াল",
                        Spoken = "Biṛāla"
                    },
                    Token = new ChapterDto.TokenDto {
                        Cefr = "A1",
                        Label = "cat",
                        TokenType = "Noun",
                        Categories = new [] { "pet", "animal" }
                    }
                }
            }
        };

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _tokens = A.Fake<IStore<Token>>();
            _translations = A.Fake<IStore<Translation>>();
            _chapters = A.Fake<IStore<GenericChapter>>();
            _importer = new ChapterDtoImporter(_database, _tokens, _translations, _chapters);
        }

        private void AssumeEmptyDatabase()
        {
            A.CallTo(() => _database.QueryTokenByLabel(A<string>._)).Returns(Task.FromResult<Token>(null));
            A.CallTo(() => _database.QueryTranslationByTokenId(A<Guid>._, A<Language>._)).Returns(Task.FromResult<Translation>(null));
            A.CallTo(() => _database.QueryChaptersByLevel(A<int>._)).Returns(Task.FromResult(Enumerable.Empty<GenericChapter>()));
        }

        private void AssumeToken()
        {
            A.CallTo(() => _tokens.Add(A<Token>._)).Returns(Task.FromResult(AToken));
        }

        private void AssertExpression(ChapterDto.ExpressionDto dto)
        {
            A.CallTo(() => _tokens.Add(A<Token>.That
                .Matches(x => x.Id == default &&
                              x.Level == Chapter.Level &&
                              x.TokenType == Enum.Parse<TokenType>(dto.Token.TokenType) &&
                              x.Label == dto.Token.Label)))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _translations.Add(A<Translation>.That
                .Matches(x => x.Id == default &&
                              x.Language == Chapter.From &&
                              x.Native == dto.Native.Native &&
                              x.Spoken == dto.Native.Spoken)))
                .MustHaveHappenedOnceExactly();
        }

        private void AssertPages(ChapterDto.PageDto[] pages)
        {
            foreach (var page in pages)
            {
                A.CallTo(() => _tokens.Add(A<Token>.That
                    .Matches(x => x.Id == default &&
                                  x.Level == Chapter.Level &&
                                  x.TokenType == Enum.Parse<TokenType>(page.Token.TokenType) &&
                                  x.Label == page.Token.Label)))
                    .MustHaveHappenedOnceExactly();

                A.CallTo(() => _translations.Add(A<Translation>.That
                    .Matches(x => x.Id == default &&
                                  x.Language == Chapter.From &&
                                  x.Native == page.From.Native &&
                                  x.Spoken == page.From.Spoken)))
                    .MustHaveHappenedOnceExactly();

                A.CallTo(() => _translations.Add(A<Translation>.That
                    .Matches(x => x.Id == default &&
                                  x.Language == Chapter.To &&
                                  x.Native == page.To.Native &&
                                  x.Spoken == page.To.Spoken)))
                    .MustHaveHappenedOnceExactly();
            }
        }

        [Test]
        public async Task Import_creates_database_entries()
        {
            AssumeEmptyDatabase();
            AssumeToken();

            await _importer.Import(Chapter);

            AssertExpression(Chapter.Name);
            AssertExpression(Chapter.Description);
            AssertPages(Chapter.Pages);
        }

        [Test]
        public async Task Import_checks_tokens_for_duplicates()
        {
            AssumeEmptyDatabase();
            AssumeToken();

            A.CallTo(() => _database.QueryTokenByLabel(A<string>._))
                .Returns(Task.FromResult(AToken));

            await _importer.Import(Chapter);

            A.CallTo(() => _tokens.Add(A<Token>._))
                .MustNotHaveHappened();
        }

        [Test]
        public async Task Import_checks_translations_for_duplicates()
        {
            AssumeEmptyDatabase();
            AssumeToken();

            A.CallTo(() => _database.QueryTranslationByTokenId(A<Guid>._, A<Language>._))
                .Returns(Task.FromResult(ATranslation));

            await _importer.Import(Chapter);

            A.CallTo(() => _translations.Add(A<Translation>._))
                .MustNotHaveHappened();
        }

        [Test]
        public async Task Import_checks_chapters_for_duplicates()
        {
            AssumeEmptyDatabase();
            AssumeToken();

            var existingChapter = new GenericChapter(
                Chapter.Level,
                AToken.Id,
                AToken.Id,
                new [] { new GenericPage(AToken.Id, PageType.OneOutOfFour) });

            A.CallTo(() => _database.QueryChaptersByLevel(A<int>._))
                .Returns(Task.FromResult(existingChapter.ToEnumeration()));

            await _importer.Import(Chapter);

            A.CallTo(() => _chapters.Add(A<GenericChapter>._))
                .MustNotHaveHappened();
        }
    }
}
