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
                await _translations.Query(new UnsupportedTranslationQuery()));
        }

        [Test]
        public async Task Query_translation_by_group_id()
        {
            var query = new TranslationQueryByGroupId(Languages.English, Languages.Bengoli, 1, TokenType.Adjective);
            var translation = TranslationDtoBuilder.Default.WithGroupId(query.GroupId).Build();

            A.CallTo(() => _database.Find(
                Names.Collections.Translations,
                A<Expression<Func<TranslationDto, bool>>>._)
            ).Returns(
                new ValueTask<IEnumerable<TranslationDto>>(new[] { translation }));

            var result = await _translations.Query(query);

            var array = result.ToArray();
            var expected = Converter.Convert(translation, Languages.English, Languages.Bengoli);

            Assert.That(array.Length, Is.EqualTo(1));
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

        private class UnsupportedTranslationQuery : TranslationQuery
        {
            public UnsupportedTranslationQuery() : base(Languages.English, Languages.Bengoli)
            {
            }
        }
    }
}
