using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
	public class TranslationProvider : ITranslationProvider
	{
        private readonly IRepository<Expression> _repository;

        public TranslationProvider(IRepository<Expression> repository)
		{
            _repository = repository;
        }

        public async Task<Translation?> Find(Guid expressionId, Language language)
        {
            var expression = await _repository.Get(expressionId);
            if (expression == null)
                return default;

            return expression
                .Translations
                .FirstOrDefault(x => x.Language == language);
        }

        public async Task<IDictionary<Guid, Translation>> FindAll(Language language, params Guid[] expressionIds)
        {
            var expressions = await _repository.GetMany(expressionIds);

            if (expressions.Length != expressionIds.Length)
                throw new InvalidOperationException(
                    $"Could not find all expressions for {string.Join(",", expressionIds)}");

            return expressions.Select(expr =>
            {
                var translation = expr.Translations.FirstOrDefault(x => x.Language == language);
                if (translation == null)
                    throw new InvalidOperationException(
                        $"Could not find {language} translation for expression {expr.Id}");

                return (id: expr.Id, translation);
            }).ToDictionary(x => x.id, x => x.translation);
        }
    }
}

