using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
    public class ExpressionManager : IExpressionManager
    {
        private readonly IRepository<Expression> _repository;

        public ExpressionManager(IRepository<Expression> repository)
        {
            _repository = repository;
        }

        public async Task<Expression> AddOrUpdate(Expression expression)
        {
            var reference = expression.Reference();
            var existingExpressions = await _repository.Find(x => x.Translations.Any(
                t => t.Language == Language.Reference && t.Native == reference));

            if (existingExpressions.Any())
            {
                var existingExpression = existingExpressions.First();
                var combinedExpression = existingExpression.Merge(expression);
                await _repository.Update(existingExpression.Id, combinedExpression);
                return combinedExpression;
            }

            return await _repository.Add(expression);
        }
    }
}

