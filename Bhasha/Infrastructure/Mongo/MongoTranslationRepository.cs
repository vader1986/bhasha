using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Infrastructure.Mongo.Dtos;
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

    private IMongoCollection<TranslationDto> GetCollection()
    {
        return _client
            .GetDatabase(_databaseName)
            .GetCollection<TranslationDto>("translations");
    }
    
    public async Task AddOrReplace(Translation translation)
    {
        if (translation.Id == Guid.Empty)
        {
            translation = translation with { Id = Guid.NewGuid() };
        }

        await GetCollection()
            .ReplaceOneAsync(x => x.ExpressionId == translation.ExpressionId &&
                                  x.Language == translation.Language,
                             translation.Convert(),
                             new ReplaceOptions { IsUpsert = true });
    }

    public ValueTask<Translation?> Find(Guid expressionId, Language language)
    {
        var result = GetCollection()
            .AsQueryable()
            .SingleOrDefault(translation => translation.ExpressionId == expressionId &&
                                            translation.Language == language);

        return new ValueTask<Translation?>(result?.Convert());
    }

    public ValueTask<Translation?> Find(string expression, Language language)
    {
        var result = GetCollection()
            .AsQueryable()
            .SingleOrDefault(translation => translation.Text == expression &&
                                            translation.Language == language);

        return new ValueTask<Translation?>(result?.Convert());
    }

    public async Task<Translation> Get(Guid translationId)
    {
        var result = await GetCollection()
            .FindAsync(x => x.Id == translationId);

        return (await result.FirstAsync()).Convert();
    }
}

