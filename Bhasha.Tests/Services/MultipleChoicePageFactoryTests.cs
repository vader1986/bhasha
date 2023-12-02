using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Tests.Services.Scenarios;
using FluentAssertions;
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
        result.Arguments.Choices.Length
            .Should()
            .Be(4);

        result.Arguments.Choices
            .Should()
            .AllSatisfy(x => x.Expression
                .Should()
                .Be(Expression.Create()));
        
        result.Arguments.Choices
            .Should()
            .ContainSingle(x => x == scenario.CorrectTranslation);
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
        result.Arguments.Choices.Length
            .Should()
            .Be(2);
        
        result.Arguments.Choices
            .Should()
            .AllSatisfy(x => x.Expression
                .Should()
                .Be(Expression.Create()));

        result.Arguments.Choices
            .Should()
            .ContainSingle(x => x == scenario.CorrectTranslation);
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
        result.Arguments.Choices.Length
            .Should()
            .Be(1);
        
        result.Arguments.Choices
            .Should()
            .AllSatisfy(x => x.Expression
                .Should()
                .Be(Expression.Create()));
        
        result.Arguments.Choices
            .Should()
            .ContainSingle(x => x == scenario.CorrectTranslation);
    }
}