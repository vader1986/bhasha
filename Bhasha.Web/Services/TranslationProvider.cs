using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
	public class TranslationProvider : ITranslationProvider
	{
        private readonly IRepository<Translation> _repository;

        public TranslationProvider(IRepository<Translation> repository)
		{
            _repository = repository;
        }

        public async Task<Translation?> Find(Guid expressionId, Language language)
        {
            var expressions = await _repository.Find(
                x => x.ExpressionId == expressionId &&
                     x.Language == language);

            return expressions?.FirstOrDefault();
        }

        public async Task<IDictionary<Guid, Translation>> FindAll(Language language, params Guid[] expressionIds)
        {
            var expressionMap = expressionIds.ToHashSet();
            var expectedLanguage = language.ToString();

            var translations = await _repository.Find(
                x => x.Language == expectedLanguage && expressionMap.Contains(x.ExpressionId));

            if (translations.Length != expressionIds.Length)
                throw new InvalidOperationException(
                    $"Could not find translations for all expressions {string.Join(",", expressionIds)}");

            return translations.ToDictionary(x => x.ExpressionId, x => x);
        }
    }
}

