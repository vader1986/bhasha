using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Extensions;
using Bhasha.Common.MongoDB.Tests.Support;
using Mongo2Go;
using MongoDB.Driver;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests
{
    [TestFixture]
    public class MongoDbLayerTests
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
        public async Task CreateUser()
        {
            var db = await MongoDb.Create(_runner.ConnectionString);
            var layer = new MongoDbLayer(db);

            var user = await layer.CreateUser(new User(default, "Hello", "asdf@bhasha.com"));

            var result = db
                .GetCollection<UserDto>(Names.Collections.Users)
                .AsQueryable()
                .Where(x => x.Id == user.Id)
                .Single();

            Assert.That(result.UserName, Is.EqualTo("Hello"));
            Assert.That(result.Email, Is.EqualTo("asdf@bhasha.com"));
        }
    }
}
