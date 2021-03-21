using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Tests.Support;
using Bhasha.Common.Tests.Support;
using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests
{
    [TestFixture]
    public class MongoDbStoreTests
    {
        private MongoDbRunner _runner;
        private MongoDb _db;
        private Converter _converter;

        [SetUp]
        public void Before()
        {
            _runner = MongoDbRunner.Start();
            _db = MongoDb.Create(_runner.ConnectionString);
            _converter = new Converter();
        }

        [TearDown]
        public void After()
        {
            _runner.Dispose();
        }

        private async Task Test_Add<TDto, TProduct>()
            where TDto : MongoDB.Dto.Dto
            where TProduct : class, IEntity
        {
            var converter = _converter as IConvert<TDto, TProduct>;
            var store = new MongoDbStore<TDto, TProduct>(_db, converter);
            var product = EntityFactory.Build<TProduct>();

            var result = await store.Add(product);

            var dto = await _db
                .GetCollection<TDto>()
                .AsQueryable()
                .SingleAsync(x => x.Id == result.Id);

            Assert.That(dto, Is.EqualTo(result));
        }

        [Test]
        public async Task Add_Products()
        {
            await Test_Add<ChapterStatsDto, ChapterStats>();
            await Test_Add<GenericChapterDto, GenericChapter>();
            await Test_Add<ProfileDto, Profile>();
            await Test_Add<TipDto, Tip>();
            await Test_Add<TokenDto, Token>();
            await Test_Add<TranslationDto, Translation>();
        }

        private async Task Test_Remove<TDto, TProduct>()
            where TProduct : class
            where TDto : MongoDB.Dto.Dto
        {
            var converter = _converter as IConvert<TDto, TProduct>;
            var store = new MongoDbStore<TDto, TProduct>(_db, converter);
            var dto = DtoFactory.Build<TDto>();

            await _db
                .GetCollection<TDto>()
                .InsertOneAsync(dto);

            var product = converter.Convert(dto);
            var deletedItems = await store.Remove(product);

            Assert.That(deletedItems, Is.EqualTo(1));

            var foundAny = await _db
                .GetCollection<ChapterStatsDto>()
                .AsQueryable()
                .AnyAsync(x => true);

            Assert.That(foundAny, Is.False);
        }

        [Test]
        public async Task Remove_Products()
        {
            await Test_Remove<ChapterStatsDto, ChapterStats>();
            await Test_Remove<GenericChapterDto, GenericChapter>();
            await Test_Remove<ProfileDto, Profile>();
            await Test_Remove<TipDto, Tip>();
            await Test_Remove<TokenDto, Token>();
            await Test_Remove<TranslationDto, Translation>();
        }

        private async Task Test_Get<TDto, TProduct>()
            where TProduct : class
            where TDto : MongoDB.Dto.Dto
        {
            var converter = _converter as IConvert<TDto, TProduct>;
            var store = new MongoDbStore<TDto, TProduct>(_db, converter);
            var dto = DtoFactory.Build<TDto>();

            await _db
                .GetCollection<TDto>()
                .InsertOneAsync(dto);

            var product = await store.Get(dto.Id);

            Assert.That(product, Is.Not.Null);
            Assert.That(dto, Is.EqualTo(product));
        }

        [Test]
        public async Task Get_Products()
        {
            await Test_Get<ChapterStatsDto, ChapterStats>();
            await Test_Get<GenericChapterDto, GenericChapter>();
            await Test_Get<ProfileDto, Profile>();
            await Test_Get<TipDto, Tip>();
            await Test_Get<TokenDto, Token>();
            await Test_Get<TranslationDto, Translation>();
        }

        private async Task Test_Replace<TDto, TProduct>()
            where TDto : MongoDB.Dto.Dto
            where TProduct : class, IEntity
        {
            var converter = _converter as IConvert<TDto, TProduct>;
            var store = new MongoDbStore<TDto, TProduct>(_db, converter);
            var dto = DtoFactory.Build<TDto>();

            await _db
                .GetCollection<TDto>()
                .InsertOneAsync(dto);

            var updatedProduct = EntityFactory.Build<TProduct>(dto.Id);

            await store.Replace(updatedProduct);

            var result = await _db
                .GetCollection<TDto>()
                .AsQueryable()
                .SingleAsync(x => x.Id == dto.Id);

            Assert.That(result, Is.EqualTo(updatedProduct));
        }

        [Test]
        public async Task Replace_Products()
        {
            await Test_Replace<ChapterStatsDto, ChapterStats>();
            await Test_Replace<GenericChapterDto, GenericChapter>();
            await Test_Replace<ProfileDto, Profile>();
            await Test_Replace<TipDto, Tip>();
            await Test_Replace<TokenDto, Token>();
            await Test_Replace<TranslationDto, Translation>();
        }
    }
}
