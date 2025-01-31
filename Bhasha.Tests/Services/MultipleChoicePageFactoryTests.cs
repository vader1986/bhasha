using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Tests.Services.Scenarios;
using Xunit;

namespace Bhasha.Tests.Services;

public class MultipleChoicePageFactoryTests
{
    [Theory, AutoData]
    public async Task PageForChapterWithMoreThanFourPages(ProfileKey key)
    {
        // arrange
        var scenario = new MultipleChoicePageFactoryScenario()
            .WithManyPages(numberOfPages: 10);
        
        // act
        var result = await scenario.Sut
            .Create(scenario.Chapter, scenario.CorrectExpression, key);
        
        // assert
        Assert.Equal(4, result.Arguments.Choices.Length);
        Assert.All(result.Arguments.Choices, x => Assert.Equal(Expression.Create(), x.Expression));
        Assert.Contains(result.Arguments.Choices, x => x == scenario.CorrectTranslation);
    }
    
    [Theory, AutoData]
    public async Task PageForChapterWithLessThanFourPages(ProfileKey key)
    {
        // arrange
        var scenario = new MultipleChoicePageFactoryScenario()
            .WithManyPages(numberOfPages: 2);
        
        // act
        var result = await scenario.Sut
            .Create(scenario.Chapter, scenario.CorrectExpression, key);
        
        // assert
        Assert.Equal(2, result.Arguments.Choices.Length);
        Assert.All(result.Arguments.Choices, x => Assert.Equal(Expression.Create(), x.Expression));
        Assert.Contains(result.Arguments.Choices, x => x == scenario.CorrectTranslation);
    }

    [Theory, AutoData]
    public async Task PageForChapterWithSinglePage(ProfileKey key)
    {
        // arrange
        var scenario = new MultipleChoicePageFactoryScenario()
            .WithManyPages(numberOfPages: 1);
        
        // act
        var result = await scenario.Sut
            .Create(scenario.Chapter, scenario.CorrectExpression, key);
        
        // assert
        Assert.Single(result.Arguments.Choices);
        Assert.All(result.Arguments.Choices, x => Assert.Equal(Expression.Create(), x.Expression));
        Assert.Contains(result.Arguments.Choices, x => x == scenario.CorrectTranslation);
    }
}