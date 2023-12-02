namespace Bhasha.Domain.Interfaces;

public interface IExpressionRepository
{
    Task<Expression> Add(Expression expression, CancellationToken token = default);
    Task<Expression> Get(int expressionId, CancellationToken token = default);
}

