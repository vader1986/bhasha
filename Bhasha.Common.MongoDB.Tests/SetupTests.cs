using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Mongo2Go;
using MongoDB.Driver;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests
{
    [TestFixture]
    public class SetupTests
    {
        private MongoDbRunner _runner;

        [SetUp]
        public void Before()
        {
            _runner = MongoDbRunner.Start();
        }

        [TearDown]
        public void After()
        {
            _runner.Dispose();
        }

        [Test]
        public async Task NewDatabase_with_mongo_db_client()
        {
            var client = new MongoClient(_runner.ConnectionString);
            var db = await Setup.NewDatabase(client);

            await AssertCollectionIndices(db, Names.Collections.Translations, 6);
            await AssertCollectionIndices(db, Names.Collections.Procedures, 3);
        }

        private async Task AssertCollectionIndices(IMongoDatabase db, string name, int expectedIndexCount)
        {
            var collection = db.GetCollection<TranslationDto>(name);
            var indices = await collection.Indexes.ListAsync();
            var indexCount = indices.ToEnumerable().Count();

            Assert.That(indexCount, Is.EqualTo(expectedIndexCount));
        }
    }
}
