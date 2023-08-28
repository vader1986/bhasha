using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Infrastructure.Mongo.Dtos;
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

    private IMongoCollection<ExpressionDto> GetCollection()
    {
        return _client
            .GetDatabase(_databaseName)
            .GetCollection<ExpressionDto>("expressions");
    }

    public async Task<Expression> Add(Expression expression)
    {
        if (expression.Id == Guid.Empty)
        {
            expression = expression with { Id = Guid.NewGuid() };
        }

        await GetCollection().InsertOneAsync(expression.Convert());

        return expression;
    }

    public async IAsyncEnumerable<Expression> Find(int level, int samples)
    {
        var options = new FindOptions<ExpressionDto, ExpressionDto>
        {
            BatchSize = samples
        };

        var cursor = await GetCollection()
            .FindAsync(x => x.Level == level, options);

        foreach (var expression in await cursor.ToListAsync())
        {
            yield return expression.Convert();
        }
    }

    public async Task<Expression> Get(Guid expressionId)
    {
        var result = await GetCollection().FindAsync(x => x.Id == expressionId);
        var item = await result.FirstAsync();
        return item.Convert();
    }
}

