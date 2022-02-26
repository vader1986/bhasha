using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services.Pages;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services.Pages
{
	public class MultipleChoicePageFactoryTests
	{
		private MultipleChoicePageFactory _factory = default!;
		private ITranslationProvider _translationProvider = default!;
		private IRepository<Expression> _expressions = default!;

		[SetUp]
		public void Before()
		{
			_translationProvider = A.Fake<ITranslationProvider>();
			_expressions = A.Fake<IRepository<Expression>>();
			_factory = new MultipleChoicePageFactory(_translationProvider, _expressions);
		}

		[Test, AutoData]
		public void GivenMissingExpression_WhenCreateAsync_ThenThrowException(Page page, Profile profile)
		{
			// setup
			A.CallTo(() => _expressions.Get(A<Guid>.Ignored)).Returns(default(Expression));

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(
				async () => await _factory.CreateAsync(page, profile));
		}

		[Test, AutoData]
		public async Task GivenPageAndProfile_WhenCreateAsync_ThenReturnDisplayedPage(
			Page page, Profile profile, Translation translation, Expression expression, Expression[] expressions)
		{
			// setup
			var translations = expressions.Select(x => x.Id).ToDictionary(x => x, _ => translation);

			A.CallTo(() => _expressions.Get(page.ExpressionId)).Returns(expression);
			A.CallTo(() => _expressions.Find(A<System.Linq.Expressions.Expression<Func<Expression, bool>>>.Ignored, 3)).Returns(expressions);
			A.CallTo(() => _translationProvider.FindAll(profile.Target, A<Guid[]>.Ignored)).Returns(translations);
			A.CallTo(() => _translationProvider.Find(page.ExpressionId, profile.Native)).Returns(translation);

			// act
			var displayedPage = await _factory.CreateAsync(page, profile);

			// verify
			Assert.NotNull(displayedPage);
			Assert.That(displayedPage.Arguments.Choices.All(x => x == translation));
		}
	}
}

