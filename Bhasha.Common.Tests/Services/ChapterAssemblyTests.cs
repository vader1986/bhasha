using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Arguments;
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
        private IAssembleArguments _argument;
        private ChapterAssembly _assembly;

        private Profile _testProfile;
        private GenericPage[] _testPages;
        private GenericChapter _testChapter;
        private Token _testToken;
        private Translation _testTranslation;
        private IArgumentAssemblyProvider _testArguments;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _tokens = A.Fake<IStore<Token>>();
            _testArguments = A.Fake<IArgumentAssemblyProvider>();
            _argument = A.Fake<IAssembleArguments>();

            A.CallTo(() => _testArguments.GetAssembly(A<PageType>._)).Returns(_argument);

            _assembly = new ChapterAssembly(_database, _tokens, _testArguments);
        }

        [Test]
        public async Task Assemble_chapter_from_chapter_id_and_profile()
        {
            AssumeAllData();

            var chapter = await _assembly.Assemble(_testChapter, _testProfile);

            Assert.That(chapter.Id == _testChapter.Id);
            Assert.That(chapter.Level == _testChapter.Level);
            Assert.That(chapter.Name == _testTranslation.Native);
            Assert.That(chapter.Description == _testTranslation.Native);
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
        public async Task Assembly_missing_token_return_null()
        {
            AssumeAllData();

            A.CallTo(() => _tokens.Get(A<Guid>._))
                .Returns(Task.FromResult<Token>(null));

            var result = await _assembly.Assemble(_testChapter, _testProfile);

            Assert.That(result, Is.Null);
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

            A.CallTo(() => _tokens.Get(A<Guid>._))
                .Returns(Task.FromResult(_testToken));

            A.CallTo(() => _database.QueryTranslationByTokenId(A<Guid>._, _testProfile.From))
                .Returns(Task.FromResult(_testTranslation));

            A.CallTo(() => _argument.Assemble(A<IEnumerable<Translation>>._, A<Guid>._))
                .Returns(_testArguments);
        }
    }
}
