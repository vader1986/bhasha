using System;
using System.Linq;
using System.Threading.Tasks;
using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using Bhasha.Common.Database;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Extensions;
using Bhasha.Common.Tests.Support;
using Mongo.Migration.Startup.Static;
using Mongo.Migration.Startup;

namespace Bhasha.Common.MongoDB.Tests
{
    [TestFixture]
    public class MongoDatabaseTests
    {
        private const string DbName = "TestDB";
        private IMongoClient _client;
        private MongoDbRunner _runner;
        private MongoDatabase _db;

        [SetUp]
        public void Before()
        {
            _runner = MongoDbRunner.Start();
            _client = new MongoClient(_runner.ConnectionString);
            _db = new MongoDatabase(_client, DbName);

            MongoMigrationClient.Initialize(_client, new MongoMigrationSettings {
                Database = DbName,
                ConnectionString = _runner.ConnectionString
            });
        }

        [TearDown]
        public void After()
        {
            MongoMigrationClient.Reset();

            _runner.Dispose();
        }

        [Test]
        public async Task QueryChapters_ForLevel_ReturnsExpectedChapters()
        {
            // setup
            var chapters = Enumerable
                .Range(1, 10)
                .Select(i => DbChapterBuilder.Default.WithLevel(i).Build());

            await _client
                .Collection<DbChapter>(DbName)
                .InsertManyAsync(chapters);

            // act
            var result = await _db.QueryChapters(5);

            // assert
            Assert.That(result.Count() == 1);
            Assert.That(result.All(x => x.Level == 5));
        }

        [Test]
        public async Task QueryProfiles_ForUserId_ReturnsExpectedProfile()
        {
            // setup
            var userId = Rnd.Create.NextString();
            var profiles = Enumerable
                .Range(1, 10)
                .Select(i => DbUserProfileBuilder
                    .Default
                    .WithUserId(i <= 5 ? userId : Rnd.Create.NextString())
                    .Build());

            await _client
                .Collection<DbUserProfile>(DbName)
                .InsertManyAsync(profiles);

            // act
            var result = await _db.QueryProfiles(userId);

            // assert
            Assert.That(result.Count() == 5);
            Assert.That(result.All(x => x.UserId == userId));
        }

        [Test]
        public async Task QueryStats_ForProfileIdAndChapterId_ReturnsExpectedStats()
        {
            // setup
            var profileId = Guid.NewGuid();
            var chapterId = Guid.NewGuid();

            var stats = Enumerable
                .Range(1, 10)
                .Select(i => DbStatsBuilder.Default
                    .WithProfileId(i == 1 ? profileId : Guid.NewGuid())
                    .WithChapterId(i < 5 ? chapterId : Guid.NewGuid())
                    .Build())
                .ToArray();

            await _client
                .Collection<DbStats>(DbName)
                .InsertManyAsync(stats);

            // act
            var result = await _db.QueryStats(chapterId, profileId);

            // assert
            Assert.That(result.ChapterId == chapterId);
            Assert.That(result.ProfileId == profileId);
            Assert.That(stats[0], Is.EqualTo(result));
        }

        [Test]
        public async Task QueryStats_ForProfileId_ReturnsExpectedStats()
        {
            // setup
            var profileId = Guid.NewGuid();
            var stats = Enumerable
                .Range(1, 10)
                .Select(i => DbStatsBuilder.Default
                    .WithProfileId(i <= 5 ? profileId : Guid.NewGuid())
                    .Build());

            await _client
                .Collection<DbStats>(DbName)
                .InsertManyAsync(stats);

            // act
            var result = await _db.QueryStats(profileId);

            // assert
            Assert.That(result.Count() == 5);
            Assert.That(result.All(x => x.ProfileId == profileId));
        }
    }
}
