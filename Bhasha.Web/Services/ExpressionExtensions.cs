using Bhasha.Web.Domain;

namespace Bhasha.Web.Services
{
	public static class ExpressionExtensions
	{
		public static string Reference(this Expression expression)
        {
			var translation = expression.Translations
				.SingleOrDefault(x => x.Language == Language.Reference);

			if (translation == null)
				throw new ArgumentException($"Expression does not a reference translation [{Language.Reference}]");

			return translation.Native;
		}

		public static Expression Merge(this Expression expression, Expression other)
		{
			var combinedTranslations = expression.Translations
				.Concat(other.Translations)
				.GroupBy(x => x.Language)
				.Select(x => x.Aggregate((a, b) => a.Merge(b)));

			return expression with
            {
				Cefr = other.Cefr != CEFR.Unknown ? other.Cefr : expression.Cefr,
				ExpressionType = other.ExpressionType,
				ResourceId = other.ResourceId ?? expression.ResourceId,
				Translations = combinedTranslations.ToArray()
            };
		}
	}
}

