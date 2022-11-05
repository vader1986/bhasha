using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services.Pages;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services.Pages;

public class MultipleChoicePageFactoryTests
{
	private readonly MultipleChoicePageFactory _factory;
	private readonly IRepository<Expression> _expressions;
	private readonly IRepository<Translation> _translations;

	public MultipleChoicePageFactoryTests()
	{
		_expressions = Substitute.For<IRepository<Expression>>();
		_translations = Substitute.For<IRepository<Translation>>();
		_factory = new MultipleChoicePageFactory(_expressions, _translations);
	}

	[Theory, AutoData]
	public void GivenMissingExpression_WhenCreateAsync_ThenThrowException(Page page, LangKey languages)
	{
		// setup
		_expressions.Get(Arg.Any<Guid>()).Returns(default(Expression));

		// act & verify
		Assert.ThrowsAsync<InvalidOperationException>(
			async () => await _factory.CreateAsync(page, languages));
	}

	[Theory, AutoData]
	public async Task GivenPageAndProfile_WhenCreateAsync_ThenReturnDisplayedPage(
		Page page, LangKey languages, Translation translation, Expression expression, Expression[] expressions)
	{
		// setup
		var translations = expressions.Select(x => x.Id).ToDictionary(x => x, _ => translation);

		_expressions.Get(page.ExpressionId).Returns(expression);
		_expressions.Find(default!, 3).ReturnsForAnyArgs(expressions);
    _translations.Find(default!).ReturnsForAnyArgs(new[] {translation});

		// act
		var displayedPage = await _factory.CreateAsync(page, languages);

		// verify
		Assert.NotNull(displayedPage);
		Assert.True(displayedPage.Arguments.Choices.All(
			x => x == translation with { ExpressionId = Guid.Empty }));
	}
}

