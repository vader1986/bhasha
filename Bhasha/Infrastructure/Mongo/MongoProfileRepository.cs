using System;
using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;

namespace Bhasha.Infrastructure.Mongo;

public class MongoProfileRepository : IProfileRepository
{
    private readonly IMongoClient _client;
    private readonly string _databaseName;

    public MongoProfileRepository(IMongoClient client, MongoSettings settings)
	{
        _client = client;
        _databaseName = settings.DatabaseName;
    }

    public async Task<Profile> Add(Profile profile)
    {
        var collection = _client.GetCollection<Profile>(_databaseName);

        if (profile.Id == Guid.Empty)
        {
            profile = profile with { Id = Guid.NewGuid() };
        }

        await collection.InsertOneAsync(profile);

        return profile;
    }

    public async IAsyncEnumerable<Profile> FindByUser(string userId)
    {
        var results = _client
            .GetCollection<Profile>(_databaseName)
            .AsQueryable()
            .Where(profile => profile.Key.UserId == userId)
            .ToAsyncEnumerable();

        await foreach (var profile in results)
        {
            yield return profile;
        }
    }

    public async Task Update(Profile profile)
    {
        await _client
            .GetCollection<Profile>(_databaseName)
            .ReplaceOneAsync(x => x.Id == profile.Id, profile);
    }
}

