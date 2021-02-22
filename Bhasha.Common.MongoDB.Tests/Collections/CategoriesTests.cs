using System;
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
        private IDatabase _database;
        private Categories _categories;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _categories = new Categories(_database);
        }

        [Test]
        public async Task List_categories_from_database()
        {
            A.CallTo(() => _database.ListMany(Names.Collections.Translations, A<Func<TranslationDto, string[]>>._))
                .Returns(new ValueTask<IEnumerable<string>>(new[] { "animals", "pets", "cats" }));

            var result = await _categories.List();

            Assert.That(result, Is.EquivalentTo(new[] {
                new Category("animals"), new Category("pets"), new Category("cats")
            }));
        }
    }
}
