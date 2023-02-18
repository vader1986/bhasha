namespace Bhasha.Domain.Interfaces;

public interface IExpressionRepository
{
    IAsyncEnumerable<Expression> Find(int level, int samples);
    Task<Expression> Add(Expression expression);
    Task<Expression> Get(Guid expressionId);
}

