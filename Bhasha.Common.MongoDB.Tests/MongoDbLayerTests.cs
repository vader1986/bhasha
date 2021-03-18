using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Tests.Support;
using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using System.Text;

namespace Bhasha.Common.MongoDB.Tests
{
    [TestFixture]
    public class MongoDbLayerTests
    {
        private MongoDbRunner _runner;
        private MongoDb _db;
        private MongoDbLayer _layer;

        [SetUp]
        public void Before()
        {
            _runner = MongoDbRunner.Start();
            _db = MongoDb.Create(_runner.ConnectionString);
            _layer = new MongoDbLayer(_db, new Converter());
        }

        [TearDown]
        public void After()
        {
            _runner.Dispose();
        }

        [Test]
        public async Task QueryChaptersByLevel()
        {
            var chapters = Enumerable
                .Range(1, 10)
                .Select(i => {
                    var dto = GenericChapterDtoBuilder.Build();
                    dto.Level = i;
                    return dto;
                });

            await _db
                .GetCollection<GenericChapterDto>()
                .InsertManyAsync(chapters);

            var result = await _layer.QueryChaptersByLevel(5);

            Assert.That(result.Count() == 5);
            Assert.That(result.All(x => x.Level <= 5));
        }

        [Test]
        public async Task QueryProfilesByUserId()
        {
            var userId = Guid.NewGuid();
            var profiles = Enumerable
                .Range(1, 10)
                .Select(i => {
                    var dto = ProfileDtoBuilder.Build();
                    dto.UserId = i <= 5 ? userId : Guid.NewGuid();
                    return dto;
                });

            await _db
                .GetCollection<ProfileDto>()
                .InsertManyAsync(profiles);

            var result = await _layer.QueryProfilesByUserId(userId);

            Assert.That(result.Count() == 5);
            Assert.That(result.All(x => x.UserId == userId));
        }

        [Test]
        public async Task QueryStatsByChapterAndProfileId()
        {
            var profileId = Guid.NewGuid();
            var chapterId = Guid.NewGuid();

            var stats = Enumerable
                .Range(1, 10)
                .Select(i => {
                    var dto = ChapterStatsDtoBuilder.Build();
                    dto.ProfileId = i == 1 ? profileId : Guid.NewGuid();
                    dto.ChapterId = i < 5 ? chapterId : Guid.NewGuid();
                    return dto;
                })
                .ToArray();

            await _db
                .GetCollection<ChapterStatsDto>()
                .InsertManyAsync(stats);

            var result = await _layer.QueryStatsByChapterAndProfileId(chapterId, profileId);

            Assert.That(result.ChapterId == chapterId);
            Assert.That(result.ProfileId == profileId);
            Assert.That(stats[0], Is.EqualTo(result));
        }

        [Test]
        public async Task QueryStatsByProfileId()
        {
            var profileId = Guid.NewGuid();
            var stats = Enumerable
                .Range(1, 10)
                .Select(i => {
                    var dto = ChapterStatsDtoBuilder.Build();
                    dto.ProfileId = i <= 5 ? profileId : Guid.NewGuid();
                    return dto;
                });

            await _db
                .GetCollection<ChapterStatsDto>()
                .InsertManyAsync(stats);

            var result = await _layer.QueryStatsByProfileId(profileId);

            Assert.That(result.Count() == 5);
            Assert.That(result.All(x => x.ProfileId == profileId));
        }

        [Test]
        public async Task QueryTips()
        {
            var chapterId = Guid.NewGuid();
            var pageIndex = 2;

            var tips = Enumerable
                .Range(1, 10)
                .Select(i => {
                    var dto = TipDtoBuilder.Build();
                    dto.ChapterId = i < 5 ? chapterId : Guid.NewGuid();
                    dto.PageIndex = i < 3 ? pageIndex : 3;
                    return dto;
                });

            await _db
                .GetCollection<TipDto>()
                .InsertManyAsync(tips);

            var result = await _layer.QueryTips(chapterId, pageIndex);

            Assert.That(result.Count() == 2);
            Assert.That(result.All(x => x.ChapterId == chapterId &&
                                        x.PageIndex == pageIndex));
        }

        [Test]
        public async Task QueryTranslationByTokenId()
        {
            var tokenId = Guid.NewGuid();
            var language = Language.English;

            var translations = Enumerable
                .Range(1, 10)
                .Select(i => {
                    var dto = TranslationDtoBuilder.Build();
                    dto.TokenId = i < 3 ? tokenId : Guid.NewGuid();
                    dto.Language = i < 2 ? language : Language.Bengoli;
                    return dto;
                })
                .ToArray();

            await _db
                .GetCollection<TranslationDto>()
                .InsertManyAsync(translations);

            var result = await _layer.QueryTranslationByTokenId(tokenId, language);

            Assert.That(translations[0], Is.EqualTo(result));
        }
    }
}
