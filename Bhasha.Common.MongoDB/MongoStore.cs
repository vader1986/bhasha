using System;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.MongoDB.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bhasha.Common.MongoDB
{
    public class MongoStore<TProduct> : IStore<TProduct>
        where TProduct : class, IEntity, ICanBeValidated
    {
        private readonly IMongoClient _client;
        private readonly string _databaseName;

        public MongoStore(IMongoClient client, string databaseName)
        {
            _client = client;
            _databaseName = databaseName;
        }

        public async Task<TProduct> Add(TProduct product)
        {
            product.Validate();
            await _client.Collection<TProduct>(_databaseName).InsertOneAsync(product);
            return product;
        }

        public async Task<TProduct?> Get(Guid id)
        {
            return await _client
                .Collection<TProduct>(_databaseName)
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Remove(Guid id)
        {
            var result = await _client
                .Collection<TProduct>(_databaseName)
                .DeleteOneAsync(x => x.Id == id);

            return (int)result.DeletedCount;
        }

        public async Task Replace(TProduct product)
        {
            await _client
                .Collection<TProduct>(_databaseName)
                .ReplaceOneAsync(x => x.Id == product.Id, product);
        }
    }
}
