using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
	public class TranslationManager : ITranslationManager
	{
		private readonly IRepository<Translation> _translations;
		private readonly IRepository<Expression> _expressions;
		private readonly IFactory<Expression> _expressionFactory;

		public TranslationManager(
			IRepository<Translation> translations,
			IRepository<Expression> expressions,
			IFactory<Expression> expressionFactory)
		{
			_translations = translations;
			_expressions = expressions;
			_expressionFactory = expressionFactory;
		}

		private async Task<Translation> GetOrAddReference(Translation reference)
        {
			var references = await _translations.Find(
				x => x.Language == reference.Language && x.Native == reference.Native);

			if (references.Any())
				return references.First();

			var expression = await _expressions.Add(_expressionFactory.Create());
			return await _translations.Add(reference with
			{
				ExpressionId = expression.Id
			});
		}

		public async Task<Translation> AddOrUpdate(Translation translation, Translation? reference)
        {
			if (translation.Language == Language.Reference)
            {
				reference = translation;
            }

			if (reference == null)
				throw new ArgumentException($"Either translation has to be in " +
                    $"{Language.Reference} or reference has to be set!");

			var existingReference = await GetOrAddReference(reference);

			if (translation.Language == Language.Reference)
            {
				return existingReference;
            }

			return await _translations.Add(translation with
			{
				ExpressionId = existingReference.ExpressionId
			});
        }
    }
}

