using System;
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
    public class MongoDbTests
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
        public async Task Find_translation_in_database()
        {
            // setup
            var client = new MongoClient(_runner.ConnectionString);
            var database = await MongoDb.Create(client);

            var db = client.GetDatabase(Names.Database);
            var collection = db.GetCollection<TranslationDto>(Names.Collections.Translations);
            var cat = TranslationDtoBuilder.Create();
            var dog = TranslationDtoBuilder.Default.WithLabel("dog").Build();

            await collection.InsertRangeAsync(cat, dog);

            // act
            var result = await database.Find<TranslationDto>(
                Names.Collections.Translations,
                x => x.Label == cat.Label, 1);

            var resultArray = result.ToArray();

            // assert
            Assert.That(resultArray.Length == 1);
            Assert.That(resultArray[0].Label == cat.Label);
            Assert.That(resultArray[0].Level == cat.Level);
            Assert.That(resultArray[0].PictureId == cat.PictureId);
            Assert.That(resultArray[0].Categories, Is.EquivalentTo(cat.Categories));
            Assert.That(resultArray[0].Tokens.Length == cat.Tokens.Length);
        }

        [Test]
        public async Task List_all_categories()
        {
            // setup
            var client = new MongoClient(_runner.ConnectionString);
            var database = await MongoDb.Create(client);

            var db = client.GetDatabase(Names.Database);
            var collection = db.GetCollection<TranslationDto>(Names.Collections.Translations);
            var cat = TranslationDtoBuilder.Default.WithCategories("animal", "life").Build();
            var tree = TranslationDtoBuilder.Default.WithLabel("tree").WithCategories("plant", "life").Build();

            await collection.InsertRangeAsync(cat, tree);

            // act
            var result = await database.ListMany<TranslationDto, string>(
                Names.Collections.Translations,
                Names.Fields.Categories);

            // assert
            Assert.That(result, Is.EquivalentTo(new[] {
                "animal", "life", "plant"
            }));
        }

        [Test]
        public async Task List_all_labels()
        {
            // setup
            var client = new MongoClient(_runner.ConnectionString);
            var database = await MongoDb.Create(client);

            var db = client.GetDatabase(Names.Database);
            var collection = db.GetCollection<TranslationDto>(Names.Collections.Translations);
            var cat = TranslationDtoBuilder.Default.WithCategories("animal", "life").Build();
            var tree = TranslationDtoBuilder.Default.WithLabel("tree").WithCategories("plant", "life").Build();

            await collection.InsertRangeAsync(cat, tree);

            // act
            var result = await database.List<TranslationDto, string>(
                Names.Collections.Translations,
                x => x.Label);

            // assert
            Assert.That(result, Is.EquivalentTo(new[] { "tree", "cat" }));
        }

        [Test]
        public async Task List_all_languages()
        {
            // setup
            var client = new MongoClient(_runner.ConnectionString);
            var database = await MongoDb.Create(client);

            var db = client.GetDatabase(Names.Database);
            var collection = db.GetCollection<TranslationDto>(Names.Collections.Translations);
            var cat = TranslationDtoBuilder.Default.WithCategories("animal", "life").Build();
            var tree = TranslationDtoBuilder.Default.WithLabel("tree").WithCategories("plant", "life").Build();

            await collection.InsertRangeAsync(cat, tree);

            // act
            var result = await database.ListMany<TranslationDto, string>(
                Names.Collections.Translations,
                Names.Fields.LanguageId);

            // assert
            Assert.That(result, Is.EquivalentTo(new[] {
                Languages.English.ToString(),
                Languages.Bengoli.ToString()
            }));
        }
    }
}
