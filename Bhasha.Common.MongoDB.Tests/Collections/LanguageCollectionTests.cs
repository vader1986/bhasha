using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Collections
{
    using Langs = MongoDB.Collections.LanguageCollection;

    [TestFixture]
    public class LanguageCollectionTests
    {
        private IMongoDb _database;
        private Langs _languages;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IMongoDb>();
            _languages = new Langs(_database);
        }

        [Test]
        public async Task List_languages_from_database()
        {
            var languages = new[] {
                Languages.English.ToString(),
                Languages.Bengoli.ToString()
            };
            
            A.CallTo(() => _database.ListMany<TranslationDto, string>(
                Names.Collections.Translations,
                Names.Fields.LanguageId)
            ).Returns(
                new ValueTask<IEnumerable<string>>(languages));

            var result = await _languages.List();

            Assert.That(result, Is.EquivalentTo(new[] {
                Languages.English, Languages.Bengoli
            }));
        }
    }
}
