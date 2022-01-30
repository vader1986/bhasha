﻿using System.Linq.Expressions;
using Bhasha.Web.Interfaces;
using MongoDB.Driver;

namespace Bhasha.Web.Services;

public class MongoRepository<T> : IRepository<T>
{
    public const string DatabaseName = "Bhasha";
	private readonly IMongoClient _client;
    private readonly string _collectionName;

	public MongoRepository(IMongoClient client)
	{
		_client = client;
        _collectionName = typeof(T).Name;
	}

    private IMongoCollection<T> GetCollection()
    {
        return _client
            .GetDatabase(DatabaseName)
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

    public async Task<T?> Get(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var items = await GetCollection().FindAsync(filter);
        return await items.FirstOrDefaultAsync();
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