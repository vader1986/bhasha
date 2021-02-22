using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Collections;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Tests.Support;
using Bhasha.Common.Queries;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Collections
{
    [TestFixture]
    public class TranslationsTests
    {
        private IMongoDb _database;
        private Translations _translations;

        internal static IEnumerable Queries
        {
            get
            {
                yield return new TestCaseData(new TranslationsCategoryQuery(1, Languages.English, Languages.Bengoli, LanguageLevel.A1, new Category("cats")));
                yield return new TestCaseData(new TranslationsTokenTypeQuery(1, Languages.English, Languages.Bengoli, LanguageLevel.A2, new Category("pets"), TokenType.Adjective));
                yield return new TestCaseData(new TranslationsLabelQuery(1, Languages.English, Languages.Bengoli, "cat"));
            }
        }

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IMongoDb>();
            _translations = new Translations(_database);
        }

        [Test]
        public void Query_unsupported()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await _translations.Query(new UnsupportedTranslationsQuery(123)));
        }

        [TestCaseSource(nameof(Queries))]
        public async Task Query_existing_procedure(TranslationsQuery query)
        {
            var translations = new[] { TranslationDtoBuilder.Create() };

            A.CallTo(() => _database.Find(
                Names.Collections.Translations,
                A<Expression<Func<TranslationDto, bool>>>._, 1)
            ).Returns(
                new ValueTask<IEnumerable<TranslationDto>>(translations));

            var result = await _translations.Query(query);

            var array = result.ToArray();
            var expected = Converter.Convert(translations[0], Languages.English, Languages.Bengoli);

            Assert.That(array.Length == 1);
            Assert.That(array[0].Reference.Label == expected.Reference.Label);
            Assert.That(array[0].Reference.Level == expected.Reference.Level);
            Assert.That(array[0].Reference.TokenType == expected.Reference.TokenType);
            Assert.That(array[0].Reference.Categories, Is.EquivalentTo(expected.Reference.Categories));

            Assert.That(array[0].From.Language == expected.From.Language);
            Assert.That(array[0].From.Native == expected.From.Native);
            Assert.That(array[0].From.Spoken == expected.From.Spoken);

            Assert.That(array[0].To.Language == expected.To.Language);
            Assert.That(array[0].To.Native == expected.To.Native);
            Assert.That(array[0].To.Spoken == expected.To.Spoken);
        }

        private class UnsupportedTranslationsQuery : TranslationsQuery
        {
            public UnsupportedTranslationsQuery(int maxItems) : base(maxItems, Languages.English, Languages.Bengoli)
            {
            }
        }
    }
}
