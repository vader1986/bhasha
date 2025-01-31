using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Tests.Services.Scenarios;
using NSubstitute;
using Xunit;

namespace Bhasha.Tests.Services;

public class AuthoringServiceTests
{
    [Theory, AutoData]
    public async Task GetOrCreateExpressionForNewExpression(string text, int level, Expression expression)
    {
        // arrange
        var scenario = new AuthoringServiceScenario();

        scenario.ExpressionRepository
            .Add(Arg.Any<Expression>())
            .Returns(expression);
        
        // act
        var result = await scenario.Sut.GetOrCreateExpression(text, level);
        
        // assert
        Assert.Equal(expression, result);

        await scenario.TranslationRepository
            .Received(1)
            .AddOrUpdate(Arg.Is<Translation>(
                x => x.Expression == expression &&
                     x.Text == text &&
                     x.Language == Language.Reference));
    }

    [Theory, AutoData]
    public async Task GetOrCreateExpressionForExistingTranslation(string text, int level)
    {
        // arrange
        var scenario = new AuthoringServiceScenario()
            .WithTranslation(text);

        // act
        var result = await scenario.Sut.GetOrCreateExpression(text, level);
        
        // assert
        Assert.Equal(scenario.Expression, result);
    }
}