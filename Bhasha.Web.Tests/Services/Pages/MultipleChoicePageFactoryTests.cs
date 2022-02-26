﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services.Pages;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services.Pages
{
	public class MultipleChoicePageFactoryTests
	{
		private readonly MultipleChoicePageFactory _factory;
		private readonly ITranslationProvider _translationProvider;
		private readonly IRepository<Expression> _expressions;

		public MultipleChoicePageFactoryTests()
		{
			_translationProvider = Substitute.For<ITranslationProvider>();
			_expressions = Substitute.For<IRepository<Expression>>();
			_factory = new MultipleChoicePageFactory(_translationProvider, _expressions);
		}

		[Theory, AutoData]
		public void GivenMissingExpression_WhenCreateAsync_ThenThrowException(Page page, Profile profile)
		{
			// setup
			_expressions.Get(Arg.Any<Guid>()).Returns(default(Expression));

			// act & verify
			Assert.ThrowsAsync<InvalidOperationException>(
				async () => await _factory.CreateAsync(page, profile));
		}

		[Theory, AutoData]
		public async Task GivenPageAndProfile_WhenCreateAsync_ThenReturnDisplayedPage(
			Page page, Profile profile, Translation translation, Expression expression, Expression[] expressions)
		{
			// setup
			var translations = expressions.Select(x => x.Id).ToDictionary(x => x, _ => translation);

			_expressions.Get(page.ExpressionId).Returns(expression);
			_expressions.Find(default!, 3).ReturnsForAnyArgs(expressions);
			_translationProvider.FindAll(profile.Target, default!).ReturnsForAnyArgs(translations);
			_translationProvider.Find(page.ExpressionId, profile.Native).Returns(translation);

			// act
			var displayedPage = await _factory.CreateAsync(page, profile);

			// verify
			Assert.NotNull(displayedPage);
			Assert.True(displayedPage.Arguments.Choices.All(
				x => x == translation with { ExpressionId = Guid.Empty }));
		}
	}
}

