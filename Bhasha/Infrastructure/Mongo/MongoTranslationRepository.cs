using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;

namespace Bhasha.Infrastructure.Mongo;

public class MongoTranslationRepository : ITranslationRepository
{
    private readonly IMongoClient _client;
    private readonly string _databaseName;

    public MongoTranslationRepository(IMongoClient client, MongoSettings settings)
	{
        _client = client;
        _databaseName = settings.DatabaseName;
    }

    public async Task AddOrUpdate(Translation translation)
    {
        var collection = _client
            .GetCollection<Translation>(_databaseName);

        if (translation.Id == Guid.Empty)
        {
            translation = translation with { Id = Guid.NewGuid() };
        }

        await collection
            .ReplaceOneAsync(x => x.ExpressionId == translation.ExpressionId &&
                                  x.Language == translation.Language,
                             translation,
                             new ReplaceOptions { IsUpsert = true });
    }

    public ValueTask<Translation?> Find(Guid expressionId, Language language)
    {
        var collection = _client
            .GetCollection<Translation>(_databaseName);

        var result = collection
            .AsQueryable()
            .Where(translation => translation.ExpressionId == expressionId &&
                                  translation.Language == language)
            .SingleOrDefault();

        return new ValueTask<Translation?>(result);
    }

    public ValueTask<Translation?> Find(string expression)
    {
        var collection = _client
            .GetCollection<Translation>(_databaseName);

        var result = collection
            .AsQueryable()
            .Where(translation => translation.Text == expression)
            .SingleOrDefault();

        return new ValueTask<Translation?>(result);
    }

    public async Task<Translation> Get(Guid translationId)
    {
        var collection = _client
            .GetCollection<Translation>(_databaseName);

        var result = await collection
            .FindAsync(x => x.Id == translationId);

        return await result.FirstAsync();
    }
}

