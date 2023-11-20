using Bhasha.MongoDb.Infrastructure.Mongo.Dtos;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using MongoDB.Driver;

namespace Bhasha.MongoDb.Infrastructure.Mongo;

public class MongoProfileRepository : IProfileRepository
{
    private readonly IMongoClient _client;
    private readonly string _databaseName;

    public MongoProfileRepository(IMongoClient client, MongoSettings settings)
	{
        _client = client;
        _databaseName = settings.DatabaseName;
    }

    private IMongoCollection<ProfileDto> GetCollection()
    {
        return _client
            .GetDatabase(_databaseName)
            .GetCollection<ProfileDto>("profiles");
    }
    
    public async Task<Profile> Add(Profile profile)
    {
        if (profile.Id == Guid.Empty)
        {
            profile = profile with { Id = Guid.NewGuid() };
        }

        await GetCollection().InsertOneAsync(profile.Convert());

        return profile;
    }

    public async IAsyncEnumerable<Profile> FindByUser(string userId)
    {
        var results = GetCollection()
            .AsQueryable()
            .Where(profile => profile.Key.UserId == userId)
            .ToAsyncEnumerable();

        await foreach (var profile in results)
        {
            yield return profile.Convert();
        }
    }

    public async Task Update(Profile profile)
    {
        await GetCollection().ReplaceOneAsync(x => x.Id == profile.Id, profile.Convert());
    }
}

