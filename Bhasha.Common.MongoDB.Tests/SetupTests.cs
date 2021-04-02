﻿using System.Linq;
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

            var dbNames = await client.ListDatabaseNames().ToListAsync();

            Assert.That(dbNames.Contains(Names.Database));

            await AssertIndices<ProfileDto>(db, Names.Collections.Profiles, 2);
            await AssertIndices<GenericChapterDto>(db, Names.Collections.Chapters, 2);
            await AssertIndices<TokenDto>(db, Names.Collections.Tokens, 2);
            await AssertIndices<TranslationDto>(db, Names.Collections.Translations, 3);
            await AssertIndices<ChapterStatsDto>(db, Names.Collections.Stats, 3);
        }

        private async Task AssertIndices<T>(IMongoDatabase db, string name, int expectedIndexCount)
        {
            var collection = db.GetCollection<T>(name);
            var indices = await collection.Indexes.ListAsync();
            var indexCount = indices.ToEnumerable().Count();

            Assert.That(indexCount, Is.EqualTo(expectedIndexCount));
        }
    }
}
