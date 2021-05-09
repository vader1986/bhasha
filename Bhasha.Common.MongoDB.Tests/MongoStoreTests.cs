using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.MongoDB.Extensions;
using Bhasha.Common.Tests.Support;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.Static;
using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests
{
    [TestFixture]
    public class MongoStoreTests
    {
        private IMongoClient _client;
        private MongoDbRunner _runner;

        [SetUp]
        public void Before()
        {
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);

            MongoMigrationClient.Initialize(_client, new MongoMigrationSettings
            {
                Database = Names.Database,
                ConnectionString = _runner.ConnectionString
            });
        }

        [TearDown]
        public void After()
        {
            MongoMigrationClient.Reset();

            _runner.Dispose();
        }

        private async Task Test_Add<TProduct>()
            where TProduct : class, IEntity, ICanBeValidated
        {
            var store = new MongoStore<TProduct>(_client);
            var product = EntityFactory.Build<TProduct>();

            var result = await store.Add(product);

            var dto = await _client
                .Collection<TProduct>()
                .AsQueryable()
                .SingleAsync(x => x.Id == result.Id);

            Assert.That(dto, Is.EqualTo(result));
        }

        [Test]
        public async Task Add_Products()
        {
            await Test_Add<DbStats>();
            await Test_Add<DbChapter>();
            await Test_Add<DbUserProfile>();
            await Test_Add<DbTranslatedChapter>();
            await Test_Add<DbExpression>();
            await Test_Add<DbWord>();
        }

        private async Task Test_Remove<TProduct>()
            where TProduct : class, IEntity, ICanBeValidated
        {
            var store = new MongoStore<TProduct>(_client);
            var product = EntityFactory.Build<TProduct>();

            await _client
                .Collection<TProduct>()
                .InsertOneAsync(product);

            var deletedItems = await store.Remove(product.Id);

            Assert.That(deletedItems, Is.EqualTo(1));

            var foundAny = await _client
                .Collection<TProduct>()
                .AsQueryable()
                .AnyAsync(x => true);

            Assert.That(foundAny, Is.False);
        }

        [Test]
        public async Task Remove_Products()
        {
            await Test_Remove<DbStats>();
            await Test_Remove<DbChapter>();
            await Test_Remove<DbUserProfile>();
            await Test_Remove<DbTranslatedChapter>();
            await Test_Remove<DbExpression>();
            await Test_Remove<DbWord>();
        }

        private async Task Test_Get<TProduct>()
            where TProduct : class, IEntity, ICanBeValidated
        {
            var store = new MongoStore<TProduct>(_client);
            var product = EntityFactory.Build<TProduct>();

            await _client
                .Collection<TProduct>()
                .InsertOneAsync(product);

            var result = await store.Get(product.Id);

            Assert.That(product, Is.Not.Null);
            Assert.That(result, Is.EqualTo(product));
        }

        [Test]
        public async Task Get_Products()
        {
            await Test_Get<DbStats>();
            await Test_Get<DbChapter>();
            await Test_Get<DbUserProfile>();
            await Test_Get<DbTranslatedChapter>();
            await Test_Get<DbExpression>();
            await Test_Get<DbWord>();
        }

        private async Task Test_Replace<TProduct>()
            where TProduct : class, IEntity, ICanBeValidated
        {
            var store = new MongoStore<TProduct>(_client);
            var product = EntityFactory.Build<TProduct>();

            await _client
                .Collection<TProduct>()
                .InsertOneAsync(product);

            var updatedProduct = EntityFactory.Build<TProduct>(product.Id);

            await store.Replace(updatedProduct);

            var result = await _client
                .Collection<TProduct>()
                .AsQueryable()
                .SingleAsync(x => x.Id == product.Id);

            Assert.That(result, Is.EqualTo(updatedProduct));
        }

        [Test]
        public async Task Replace_Products()
        {
            await Test_Replace<DbStats>();
            await Test_Replace<DbChapter>();
            await Test_Replace<DbUserProfile>();
            await Test_Replace<DbTranslatedChapter>();
            await Test_Replace<DbExpression>();
            await Test_Replace<DbWord>();
        }
    }
}
