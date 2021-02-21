using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Extensions;
using Bhasha.Common.Queries;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Queries
{
    [TestFixture]
    public class StorageTests
    {
        private static readonly UserProgress AUserProgress = new UserProgress(new EntityId("user123"), Languages.English, Languages.Bengoli, LanguageLevel.A1, new[] { new Category("animal") });
        private static readonly Procedure AProcedure = new Procedure(new ProcedureId("test"), "test", null, null, new TokenType[0]);
        private static readonly Translation ATranslation = new Translation(new Token("test", new[] { new Category("test") }, LanguageLevel.A1, TokenType.Noun), new LanguageToken(Languages.English, "test", "test"), new LanguageToken(Languages.Bengoli, "test", "test"));

        private IListable<Category> _categories;
        private IListable<ProcedureId> _procedureIds;
        private IQueryable<Procedure, ProcedureQuery> _procedures;
        private IQueryable<Translation, TranslationsQuery> _translations;
        private IStorage _storage;

        [SetUp]
        public void Before()
        {
            _categories = A.Fake<IListable<Category>>();
            _procedureIds = A.Fake<IListable<ProcedureId>>();
            _procedures = A.Fake<IQueryable<Procedure, ProcedureQuery>>();
            _translations = A.Fake<IQueryable<Translation, TranslationsQuery>>();

            _storage = new Storage(_categories, _procedureIds, _procedures, _translations);
        }

        [Test]
        public async Task FetchFor_no_unfinished_catregories_returns_default()
        {
            A.CallTo(() => _categories.List()).Returns(new ValueTask<IEnumerable<Category>>(AUserProgress.Finished));
            A.CallTo(() => _procedureIds.List()).Returns(new ValueTask<IEnumerable<ProcedureId>>(new[] { new ProcedureId("test") }));
            A.CallTo(() => _procedures.Query(A<ProcedureQuery>._)).Returns(new ValueTask<IEnumerable<Procedure>>(AProcedure.ToEnumeration()));

            var result = await _storage.FetchFor(AUserProgress);

            A.CallTo(() => _categories.List()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _procedureIds.List()).MustHaveHappenedOnceExactly();

            Assert.That(result == default);
        }

        [Test]
        public void FetchFor_no_procedure_found()
        {
            A.CallTo(() => _categories.List()).Returns(new ValueTask<IEnumerable<Category>>(new[] { new Category("test") }));
            A.CallTo(() => _procedureIds.List()).Returns(new ValueTask<IEnumerable<ProcedureId>>(new[] { new ProcedureId("test") }));
            A.CallTo(() => _procedures.Query(A<ProcedureQuery>._)).Returns(new ValueTask<IEnumerable<Procedure>>(new Procedure[0]));

            Assert.ThrowsAsync<NoProcedureFoundException>(async () => await _storage.FetchFor(AUserProgress));
        }

        [Test]
        public void FetchFor_no_translation_found()
        {
            A.CallTo(() => _categories.List()).Returns(new ValueTask<IEnumerable<Category>>(new[] { new Category("test") }));
            A.CallTo(() => _procedureIds.List()).Returns(new ValueTask<IEnumerable<ProcedureId>>(new[] { new ProcedureId("test") }));
            A.CallTo(() => _procedures.Query(A<ProcedureQuery>._)).Returns(new ValueTask<IEnumerable<Procedure>>(AProcedure.ToEnumeration()));
            A.CallTo(() => _translations.Query(A<TranslationsQuery>._)).Returns(new ValueTask<IEnumerable<Translation>>(new Translation[0]));

            Assert.ThrowsAsync<NoTranslationFoundException>(async () => await _storage.FetchFor(AUserProgress));
        }

        [Test]
        public async Task FetchFor_valid_state_and_user_progress()
        {
            A.CallTo(() => _categories.List()).Returns(new ValueTask<IEnumerable<Category>>(new[] { new Category("test") }));
            A.CallTo(() => _procedureIds.List()).Returns(new ValueTask<IEnumerable<ProcedureId>>(new[] { new ProcedureId("test") }));
            A.CallTo(() => _procedures.Query(A<ProcedureQuery>._)).Returns(new ValueTask<IEnumerable<Procedure>>(AProcedure.ToEnumeration()));
            A.CallTo(() => _translations.Query(A<TranslationsQuery>._)).Returns(new ValueTask<IEnumerable<Translation>>(ATranslation.ToEnumeration()));

            var result = await _storage.FetchFor(AUserProgress);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Pool, Is.EquivalentTo(ATranslation.ToEnumeration()));
            Assert.That(result.Procedure, Is.EqualTo(AProcedure));
        }
    }
}
