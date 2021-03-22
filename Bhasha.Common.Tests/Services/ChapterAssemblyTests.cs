using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Arguments;
using Bhasha.Common.Exceptions;
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

        private Profile _testProfile;
        private GenericPage[] _testPages;
        private GenericChapter _testChapter;
        private Token _testToken;
        private Translation _testTranslation;
        private object _testArguments;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _tokens = A.Fake<IStore<Token>>();
            _chapters = A.Fake<IStore<GenericChapter>>();
            _argument = A.Fake<IAssembleArguments>();
            _assembly = new ChapterAssembly(_database, _tokens, _chapters, _ => _argument);
        }

        [Test]
        public async Task Assemble_chapter_from_chapter_id_and_profile()
        {
            AssumeAllData();

            var chapter = await _assembly.Assemble(_testChapter.Id, _testProfile);

            Assert.That(chapter.Id == _testChapter.Id);
            Assert.That(chapter.Level == _testChapter.Level);
            Assert.That(chapter.Name == _testChapter.Name);
            Assert.That(chapter.Description == _testChapter.Description);
            Assert.That(chapter.Pages.Length == _testChapter.Pages.Length);

            for (int i = 0; i < chapter.Pages.Length; i++)
            {
                Assert.That(chapter.Pages[i].PageType == _testChapter.Pages[i].PageType);
                Assert.That(chapter.Pages[i].Token == _testToken);
                Assert.That(chapter.Pages[i].Translation == _testTranslation);
                Assert.That(chapter.Pages[i].Arguments == _testArguments);
            }
        }

        [Test]
        public void Assembly_missing_chapter_throws()
        {
            var profile = ProfileBuilder
                .Default
                .WithFrom(Language.Bengali)
                .WithTo(Language.English)
                .Build();

            var chapterId = Guid.NewGuid();
            
            A.CallTo(() => _chapters.Get(chapterId))
                .Returns(Task.FromResult<GenericChapter>(null));

            Assert.ThrowsAsync<ObjectNotFoundException>(async () => await _assembly.Assemble(chapterId, profile));
        }

        [Test]
        public void Assembly_missing_token_throws()
        {
            AssumeAllData();

            A.CallTo(() => _tokens.Get(A<Guid>._))
                .Returns(Task.FromResult<Token>(null));

            Assert.ThrowsAsync<ObjectNotFoundException>(async () => await _assembly.Assemble(_testChapter.Id, _testProfile));
        }

        private void AssumeAllData()
        {
            _testProfile = ProfileBuilder
                .Default
                .WithFrom(Language.Bengali)
                .WithTo(Language.English)
                .Build();

            var page = GenericPageBuilder
                .Default
                .Build();

            _testPages = Enumerable
                .Range(0, 5)
                .Select(_ => page).ToArray();

            _testChapter = GenericChapterBuilder
                .Default
                .WithPages(_testPages)
                .Build();

            _testToken = TokenBuilder
                .Default
                .WithId(page.TokenId)
                .Build();

            _testTranslation = TranslationBuilder
                .Default
                .WithTokenId(page.TokenId)
                .Build();

            _testArguments = new object();

            A.CallTo(() => _chapters.Get(_testChapter.Id))
                .Returns(Task.FromResult(_testChapter));

            A.CallTo(() => _tokens.Get(A<Guid>._))
                .Returns(Task.FromResult(_testToken));

            A.CallTo(() => _database.QueryTranslationByTokenId(A<Guid>._, _testProfile.From))
                .Returns(Task.FromResult(_testTranslation));

            A.CallTo(() => _argument.Assemble(A<IEnumerable<Translation>>._, A<Guid>._))
                .Returns(_testArguments);
        }
    }
}
