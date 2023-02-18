using System;
using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;

namespace Bhasha.Infrastructure.Mongo;

public class MongoExpressionRepository : IExpressionRepository
{
    private readonly IMongoClient _client;
    private readonly string _databaseName;

    public MongoExpressionRepository(IMongoClient client, MongoSettings settings)
    {
        _client = client;
        _databaseName = settings.DatabaseName;
    }

    public async Task<Expression> Add(Expression expression)
    {
        var collection = _client.GetCollection<Expression>(_databaseName);

        if (expression.Id == Guid.Empty)
        {
            expression = expression with { Id = Guid.NewGuid() };
        }

        await collection.InsertOneAsync(expression);

        return expression;
    }

    public async IAsyncEnumerable<Expression> Find(int level, int samples)
    {
        var collection = _client.GetCollection<Expression>(_databaseName);
        var options = new FindOptions<Expression, Expression>
        {
            BatchSize = samples
        };

        var curser = await collection.FindAsync(x => x.Level == level, options);

        foreach (var expression in await curser.ToListAsync())
        {
            yield return expression;
        }
    }

    public async Task<Expression> Get(Guid expressionId)
    {
        var collection = _client.GetCollection<Expression>(_databaseName);
        var result = await collection.FindAsync(x => x.Id == expressionId);

        return await result.FirstAsync();
    }
}

