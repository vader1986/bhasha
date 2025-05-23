using Bhasha.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Expression = Bhasha.Domain.Expression;

namespace Bhasha.Infrastructure.EntityFramework;

public sealed class EntityFrameworkExpressionRepository(AppDbContext context) : IExpressionRepository
{
    public async Task<Expression> Add(Expression expression, CancellationToken token = default)
    {
        var result = await context.Expressions
            .AddAsync(expression.ToEntityFramework(), token);
        
        await context.SaveChangesAsync(token);
        
        return result.Entity.ToDomain();
    }

    public async Task<Expression> AddOrUpdate(Expression expression, CancellationToken token = default)
    {
        if (expression.Id == 0)
        {
            return await Add(expression, token);
        }

        var dto = await context.Expressions
            .SingleAsync(x => x.Id == expression.Id, token);
        
        dto.Level = expression.Level;
        dto.ExpressionType = expression.ExpressionType?.ToEntityFramework();
        dto.PartOfSpeech = expression.PartOfSpeech?.ToEntityFramework();
        dto.Cefr = expression.Cefr?.ToEntityFramework();
        dto.ResourceId = expression.ResourceId;
        dto.Labels = expression.Labels.ToArray();
        dto.Synonyms = expression.Synonyms.ToArray();

        context.Expressions.Update(dto);
        
        await context.SaveChangesAsync(token);
        
        return dto.ToDomain();
    }

    public async Task<Expression> Get(int expressionId, CancellationToken token = default)
    {
        return (await context.Expressions
            .FirstAsync(x => x.Id == expressionId, token))
            .ToDomain();
    }
}