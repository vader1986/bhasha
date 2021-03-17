using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class ChapterAssemblyTests
    {
        private IDatabase _database;
        private IStore<Token> _tokens;
        private IStore<GenericChapter> _chapters;
        private IAssembleArguments _argument;
        private ChapterAssembly _assembly;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _tokens = A.Fake<IStore<Token>>();
            _chapters = A.Fake<IStore<GenericChapter>>();
            _argument = A.Fake<IAssembleArguments>();
            _assembly = new ChapterAssembly(_database, _tokens, _chapters, new[] { _argument });
        }

        [Test]
        public async Task Assemble_chapter_from_chapter_id_and_profile()
        {
            var profile = ProfileBuilder
                .Default
                .WithFrom(Language.Bengoli)
                .WithTo(Language.English)
                .Build();

            var page = GenericPageBuilder
                .Default
                .Build();

            var genericPages = Enumerable
                .Range(0, 5)
                .Select(_ => page).ToArray();

            var genericChapter = GenericChapterBuilder
                .Default
                .WithPages(genericPages)
                .Build();

            var token = TokenBuilder
                .Default
                .WithId(page.TokenId)
                .Build();

            var translation = TranslationBuilder
                .Default
                .WithTokenId(page.TokenId)
                .Build();

            var arguments = new object();

            AssumeAllDataForChapter(genericChapter, profile.From, token, translation, arguments);

            var chapter = await _assembly.Assemble(genericChapter.Id, profile);

            Assert.That(chapter.Id == genericChapter.Id);
            Assert.That(chapter.Level == genericChapter.Level);
            Assert.That(chapter.Name == genericChapter.Name);
            Assert.That(chapter.Description == genericChapter.Description);
            Assert.That(chapter.Pages.Length == genericChapter.Pages.Length);

            for (int i = 0; i < chapter.Pages.Length; i++)
            {
                Assert.That(chapter.Pages[i].PageType == genericChapter.Pages[i].PageType);
                Assert.That(chapter.Pages[i].Token == token);
                Assert.That(chapter.Pages[i].Translation == translation);
                Assert.That(chapter.Pages[i].Arguments == arguments);
            }
        }

        private void AssumeAllDataForChapter(GenericChapter genericChapter, Language from, Token token, Translation translation, object arguments)
        {
            A.CallTo(() => _chapters.Get(genericChapter.Id))
                .Returns(Task.FromResult(genericChapter));

            A.CallTo(() => _tokens.Get(A<Guid>._)).Returns(Task.FromResult(token));
            A.CallTo(() => _database.QueryTranslationByTokenId(A<Guid>._, from)).Returns(Task.FromResult(translation));
            A.CallTo(() => _argument.Supports(PageType.OneOutOfFour)).Returns(true);
            A.CallTo(() => _argument.Assemble(A<IEnumerable<Translation>>._, A<Guid>._)).Returns(arguments);
        }
    }
}
