using System;
using System.Linq;
using Bhasha.Web.Domain;

namespace Bhasha.Web.Tests.Support
{
	public static class ExpressionBuilder
	{
		public static Expression WithReference(this Expression expression)
        {
			var translations = expression.Translations
				.Where(x => x.Language != Language.Reference)
				.Append(new Translation(Language.Reference, "native", default, default))
				.ToArray();

			return expression with {
				Translations = translations
			};
        }

		public static Expression WithoutReference(this Expression expression)
        {
			var translations = expression.Translations
				.Where(x => x.Language != Language.Reference)
				.ToArray();

			return expression with
			{
				Translations = translations
			};
		}
	}
}

