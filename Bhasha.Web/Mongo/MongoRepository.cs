using System.Linq.Expressions;
using Bhasha.Web.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bhasha.Web.Mongo;

public class MongoRepository<T> : IRepository<T>
{
	private readonly IMongoClient _client;
    private readonly string _collectionName;
    private readonly string _databaseName;

	public MongoRepository(IMongoClient client, MongoSettings settings)
	{
		_client = client;
        _collectionName = typeof(T).Name;
        _databaseName = settings.DatabaseName;
    }

    private IMongoCollection<T> GetCollection()
    {
        return _client
            .GetDatabase(_databaseName)
            .GetCollection<T>(_collectionName);
    }

    public async Task<T> Add(T item)
    {
        await GetCollection().InsertOneAsync(item);
        return item;
    }

    public async Task<T[]> Find(Expression<Func<T, bool>> query)
    {
        var items = await GetCollection().FindAsync(query);
        return items.ToEnumerable().ToArray();
    }

    public async Task<T[]> Find(Expression<Func<T, bool>> query, int samples)
    {
        var items = await GetCollection().AsQueryable().Where(query).Sample(samples).ToListAsync();
        return items.ToArray();
    }

    public async Task<T?> Get(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var items = await GetCollection().FindAsync(filter);
        return await items.FirstOrDefaultAsync();
    }

    public async Task<T[]> GetMany(params Guid[] ids)
    {
        var filter = Builders<T>.Filter.In("_id", ids);
        var result = await GetCollection().FindAsync(filter);
        var items = await result.ToListAsync();
        return items.ToArray();
    }

    public async Task<bool> Remove(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await GetCollection().DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task<bool> Update(Guid id, T item)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await GetCollection().ReplaceOneAsync(filter, item);
        return result.MatchedCount > 0;
    }
}