using Bhasha.Shared.Domain;
using Bhasha.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bhasha.Infrastructure.EntityFramework;

public class EntityFrameworkExpressionRepository(AppDbContext context) : IExpressionRepository
{
    public IAsyncEnumerable<Expression> Find(int level, int samples, CancellationToken token = default)
    {
        return context.Expressions
            .Where(x => x.Level == level)
            .Take(samples)
            .Select(x => Converter.Convert(x))
            .ToAsyncEnumerable();
    }

    public async Task<Expression> Add(Expression expression, CancellationToken token = default)
    {
        var result = await context.Expressions
            .AddAsync(Converter.Convert(expression), token);
        
        await context
            .SaveChangesAsync(token);
        
        return Converter
            .Convert(result.Entity);
    }

    public async Task<Expression> Get(int expressionId, CancellationToken token = default)
    {
        return Converter
            .Convert(await context.Expressions
            .FirstAsync(x => x.Id == expressionId, token));
    }
}