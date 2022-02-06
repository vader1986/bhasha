using System;
using System.Linq;
using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Services;
using Bhasha.Web.Tests.Support;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
	[TestFixture]
	public class ExpressionExtensionsTests
	{
		[Test, AutoData]
		public void GivenExpressionWithoutReferenceTranslation_WhenCallReference_ThenThrow(Expression expression)
		{
			Assert.Throws<ArgumentException>(() => expression.WithoutReference().Reference());
		}

		[Test, AutoData]
		public void GivenExpression_WhenCallReference_ThenReturnReferenceString(Expression expression)
        {
			Assert.NotNull(expression.WithReference().Reference());
        }

		[Test, AutoData]
		public void GivenExpression_WhenMergedWithOtherExpression_ThenReturnCombinedTranslations(Expression lhs, Expression rhs)
		{
			lhs = lhs with { Translations = new[]
			{
				new Translation(Language.English, "very", default, "very.wav")
			}};

			rhs = rhs with { Translations = new[]
			{
				new Translation(Language.English, "very", "very", default),
				new Translation(Language.Bengali, "khub", default, default)
			}};

			var combined = lhs.Merge(rhs);

			Assert.That(combined.Translations, Is.EquivalentTo(new[]
            {
				new Translation(Language.English, "very", "very", "very.wav"),
				new Translation(Language.Bengali, "khub", default, default)
			}));
		}

		[Test, AutoData]
		public void GivenExpression_WhenMergedWithOtherExpression_ThenReturnNonNullResourceId(Expression lhs, Expression rhs)
		{
			lhs = lhs with { ResourceId = null };
			rhs = rhs with { ResourceId = "test.bmp" };

			var combined = lhs.Merge(rhs);

			Assert.That(combined.ResourceId, Is.EqualTo("test.bmp"));
		}

		[Test, AutoData]
		public void GivenExpressionWithUnkownCefr_WhenMergedWithOtherExpression_ThenReturnWithUpdatedCefr(Expression lhs, Expression rhs)
		{
			lhs = lhs with { Cefr = CEFR.Unknown };
			rhs = rhs with { Cefr = CEFR.A1 };

			var combined = lhs.Merge(rhs);

			Assert.That(combined.Cefr, Is.EqualTo(CEFR.A1));
		}
	}
}

