using System;
using Bhasha.Web.Services;
using Xunit;

namespace Bhasha.Web.Tests.Services
{
	public class ExpressionFactoryTests
	{
		[Fact]
		public void CreateNewExpression()
		{
			var factory = new ExpressionFactory();
			var expression = factory.Create();

			Assert.Equal(expression.Id, Guid.Empty);
			Assert.Equal(expression.Labels, Array.Empty<string>());
			Assert.Equal(0, expression.Level);
			Assert.Null(expression.ResourceId);
			Assert.Null(expression.Cefr);
			Assert.Null(expression.ExpressionType);
			Assert.Null(expression.PartOfSpeech);
		}
	}
}

