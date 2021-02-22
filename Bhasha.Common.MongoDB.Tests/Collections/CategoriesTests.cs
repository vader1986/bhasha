using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Collections;
using Bhasha.Common.MongoDB.Dto;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Collections
{
    [TestFixture]
    public class CategoriesTests
    {
        private IMongoDb _database;
        private Categories _categories;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IMongoDb>();
            _categories = new Categories(_database);
        }

        [Test]
        public async Task List_categories_from_database()
        {
            var categories = new[] { "animals", "pets", "cats" };

            A.CallTo(() => _database.ListMany<TranslationDto, string>(
                Names.Collections.Translations,
                Names.Fields.Categories)
            ).Returns(
                new ValueTask<IEnumerable<string>>(categories));

            var result = await _categories.List();

            Assert.That(result, Is.EquivalentTo(new[] {
                new Category("animals"), new Category("pets"), new Category("cats")
            }));
        }
    }
}
