using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.Aggregation;
using Bhasha.Common.Queries;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Aggregation
{
    [TestFixture]
    public class CategoryLoaderTests
    {
        private IListable<Category> _categories;
        private CategoryLoader _loader;

        [SetUp]
        public void Before()
        {
            _categories = A.Fake<IListable<Category>>();
            _loader = new CategoryLoader(_categories);
        }

        [Test]
        public async Task NextCategory_without_unfinished_categories()
        {
            var finishedCategories = new[] { new Category("animals") };

            var userProgress = UserProgressBuilder
                .Default
                .WithFinished(finishedCategories)
                .Build();

            A.CallTo(() => _categories.List()).Returns(new ValueTask<IEnumerable<Category>>(finishedCategories));

            var result = await _loader.NextCategory(userProgress);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task NextCategory_with_unfinished_category()
        {
            var finishedCategories = new[] { new Category("animals") };

            var userProgress = UserProgressBuilder
                .Default
                .WithFinished(finishedCategories)
                .Build();

            A.CallTo(() => _categories.List()).Returns(new ValueTask<IEnumerable<Category>>(
                new[] { new Category("animals"), new Category("pets") }));

            var result = await _loader.NextCategory(userProgress);

            Assert.That(result, Is.EqualTo(new Category("pets")));
        }
    }
}
