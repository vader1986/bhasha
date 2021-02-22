using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Collections;
using Bhasha.Common.MongoDB.Dto;
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
            var translation = new TranslationDto
            {
                Categories = new[] { "cats", "pets" },
                Label = "Cats are pets.",
                Level = LanguageLevel.A1.ToString(),
                TokenType = TokenType.Phrase.ToString(),
                Tokens = new[] {
                    new TokenDto
                    {
                        LanguageId = Languages.English,
                        Native = "Cats are pets.",
                        Spoken = "Cats are pets."
                    },
                    new TokenDto
                    {
                        LanguageId = Languages.Bengoli,
                        Native = "???",
                        Spoken = "???"
                    }
                }
            };

            A.CallTo(() => _database.Find(Names.Collections.Translations, A<Expression<Func<TranslationDto, bool>>>._, 1))
                .Returns(new ValueTask<IEnumerable<TranslationDto>>(new[] { translation }));

            var result = await _translations.Query(query);

            var array = result.ToArray();

            Assert.That(array.Length == 1);
            Assert.That(array[0].Reference.Label == translation.Label);
            Assert.That(array[0].Reference.Level == LanguageLevel.A1);
            Assert.That(array[0].Reference.TokenType == TokenType.Phrase);
            Assert.That(array[0].Reference.Categories, Is.EquivalentTo(new[] {
                new Category("cats"), new Category("pets")
            }));

            Assert.That(array[0].From.Language == Languages.English);
            Assert.That(array[0].From.Native == "Cats are pets.");
            Assert.That(array[0].From.Spoken == "Cats are pets.");

            Assert.That(array[0].To.Language == Languages.Bengoli);
            Assert.That(array[0].To.Native == "???");
            Assert.That(array[0].To.Spoken == "???");
        }

        private class UnsupportedTranslationsQuery : TranslationsQuery
        {
            public UnsupportedTranslationsQuery(int maxItems) : base(maxItems, Languages.English, Languages.Bengoli)
            {
            }
        }
    }
}
