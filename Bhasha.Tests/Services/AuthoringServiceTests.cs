using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Shared.Domain;
using Bhasha.Tests.Services.Scenarios;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Bhasha.Tests.Services;

public class AuthoringServiceTests
{
    [Theory, AutoData]
    public async Task GetExpressionIdForNewExpression(string text, int level, Expression expression)
    {
        // arrange
        var scenario = new AuthoringServiceScenario();

        scenario.ExpressionRepository
            .Add(Arg.Any<Expression>())
            .Returns(expression);
        
        // act
        var result = await scenario.Sut.GetExpressionId(text, level);
        
        // assert
        result
            .Should()
            .Be(expression.Id);

        await scenario.TranslationRepository
            .Received(1)
            .AddOrReplace(Arg.Is<Translation>(
                x => x.ExpressionId == expression.Id &&
                     x.Text == text &&
                     x.Language == Language.Reference));
    }

    [Theory, AutoData]
    public async Task GetExpressionIdForExistingTranslation(string text, int level)
    {
        // arrange
        var scenario = new AuthoringServiceScenario()
            .WithTranslation(text);

        // act
        var result = await scenario.Sut.GetExpressionId(text, level);
        
        // assert
        result
            .Should()
            .Be(scenario.ExpressionId);
    }
}