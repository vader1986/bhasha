using System;
using Bhasha.Web.Services;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
	public class ExpressionFactoryTests
	{
		[Test]
		public void CreateNewExpression()
		{
			var factory = new ExpressionFactory();
			var expression = factory.Create();

			Assert.That(expression.Id, Is.EqualTo(Guid.Empty));
			Assert.That(expression.Labels, Is.EqualTo(Array.Empty<string>()));
			Assert.That(expression.Level, Is.EqualTo(0));
			Assert.That(expression.ResourceId, Is.EqualTo(null));
			Assert.That(expression.Cefr, Is.EqualTo(null));
			Assert.That(expression.ExpressionType, Is.EqualTo(null));
			Assert.That(expression.PartOfSpeech, Is.EqualTo(null));
		}
	}
}

