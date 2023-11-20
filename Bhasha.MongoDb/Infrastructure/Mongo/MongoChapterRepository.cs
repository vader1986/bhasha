using Bhasha.MongoDb.Infrastructure.Mongo.Dtos;
using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using MongoDB.Driver;

namespace Bhasha.MongoDb.Infrastructure.Mongo;

public class MongoChapterRepository : IChapterRepository
{
    private readonly IMongoClient _client;
    private readonly string _databaseName;

    public MongoChapterRepository(IMongoClient client, MongoSettings settings)
	{
        _client = client;
        _databaseName = settings.DatabaseName;
    }

    private IMongoCollection<ChapterDto> GetCollection()
    {
        return _client
            .GetDatabase(_databaseName)
            .GetCollection<ChapterDto>("chapters");
    }

    public async Task<Chapter> AddOrReplace(Chapter chapter)
    {
        var collection = GetCollection();
        var dto = chapter.Convert();
        
        if (dto.Id == Guid.Empty)
        {
            dto = dto with { Id = Guid.NewGuid() };
            await collection.InsertOneAsync(dto);
        }
        else
        {
            var options = new ReplaceOptions { IsUpsert = true };
            await collection.ReplaceOneAsync(x => x.Id == dto.Id, dto, options);
        }

        return dto.Convert();
    }

    public ValueTask<Chapter?> FindById(Guid chapterId)
    {
        var result = GetCollection()
            .AsQueryable()
            .SingleOrDefault(chapter => chapter.Id == chapterId);

        if (result is null)
        {
            return new ValueTask<Chapter?>();
        }
        
        return new ValueTask<Chapter?>(result.Convert());
    }

    public async IAsyncEnumerable<Chapter> FindByLevel(int level)
    {
        var results = GetCollection()
            .AsQueryable()
            .Where(chapter => chapter.RequiredLevel == level)
            .ToAsyncEnumerable();

        await foreach (var chapter in results)
        {
            yield return chapter.Convert();
        }
    }
}

