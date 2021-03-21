using System;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Services;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bhasha.Common.MongoDB
{
    public class MongoDbStore<TDto, TProduct> : IStore<TProduct>
        where TProduct : class
        where TDto : Dto.Dto
    {
        private readonly IMongoDb _db;
        private readonly IConvert<TDto, TProduct> _converter;

        public MongoDbStore(IMongoDb db, IConvert<TDto, TProduct> converter)
        {
            _db = db;
            _converter = converter;
        }

        public async Task<TProduct> Add(TProduct product)
        {
            var dto = _converter.Convert(product);
            await _db.GetCollection<TDto>().InsertOneAsync(dto);

            return _converter.Convert(dto);
        }

        public async Task<TProduct?> Get(Guid id)
        {
            var dto = await _db
                .GetCollection<TDto>()
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

            return dto != null ? _converter.Convert(dto) : default;
        }

        public async Task<int> Remove(TProduct product)
        {
            var dto = _converter.Convert(product);
            var result = await _db
                .GetCollection<TDto>()
                .DeleteOneAsync(x => x.Id == dto.Id);

            return (int)result.DeletedCount;
        }

        public async Task Replace(TProduct product)
        {
            var dto = _converter.Convert(product);
            await _db
                .GetCollection<TDto>()
                .ReplaceOneAsync(x => x.Id == dto.Id, dto);
        }
    }
}
