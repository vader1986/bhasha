using Bhasha.Web.Domain;
using Bhasha.Web.Domain.Interfaces;
using Bhasha.Web.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;

namespace Bhasha.Web.Infrastructure.Mongo;

public class MongoChapterRepository : IChapterRepository
{
    private readonly IMongoClient _client;
    private readonly string _databaseName;

    public MongoChapterRepository(IMongoClient client, MongoSettings settings)
	{
        _client = client;
        _databaseName = settings.DatabaseName;
    }

    public async Task<Chapter> Upsert(Chapter chapter)
    {
        var collection = _client.GetCollection<Chapter>(_databaseName);

        if (chapter.Id == Guid.Empty)
        {
            chapter = chapter with { Id = Guid.NewGuid() };
            await collection.InsertOneAsync(chapter);
        }
        else
        {
            await collection.ReplaceOneAsync(x => x.Id == chapter.Id, chapter);
        }

        return chapter;
    }

    public ValueTask<Chapter?> Find(Guid chapterId)
    {
        var collection = _client
            .GetCollection<Chapter>(_databaseName);

        var result = collection
            .AsQueryable()
            .Where(chapter => chapter.Id == chapterId)
            .SingleOrDefault();

        return new ValueTask<Chapter?>(result);
    }

    public async IAsyncEnumerable<Chapter> GetChapters(int level)
    {
        var collection = _client
            .GetCollection<Chapter>(_databaseName);

        var results = collection
            .AsQueryable()
            .Where(chapter => chapter.RequiredLevel == level)
            .ToAsyncEnumerable();

        await foreach (var chapter in results)
        {
            yield return chapter;
        }
    }
}

