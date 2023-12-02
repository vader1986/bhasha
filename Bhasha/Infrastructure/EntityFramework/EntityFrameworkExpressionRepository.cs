using Bhasha.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Expression = Bhasha.Domain.Expression;

namespace Bhasha.Infrastructure.EntityFramework;

public class EntityFrameworkExpressionRepository(AppDbContext context) : IExpressionRepository
{
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