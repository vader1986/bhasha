namespace Bhasha.Shared.Domain.Interfaces;

public interface IExpressionRepository
{
    IAsyncEnumerable<Expression> Find(int level, int samples, CancellationToken token = default);
    Task<Expression> Add(Expression expression, CancellationToken token = default);
    Task<Expression> Get(int expressionId, CancellationToken token = default);
}

