using System;
using System.Linq;
using System.Threading;
using Bhasha.Domain;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public static class MultipleChoicePageFactoryScenarioExtensions
{
    public static MultipleChoicePageFactoryScenario WithManyPages(this MultipleChoicePageFactoryScenario scenario, int numberOfPages)
    {
        var correctId = Random.Shared.Next() % numberOfPages + 1;
        
        scenario.CorrectExpression = Expression.Create() with
        {
            Id = correctId
        };
        
        scenario.Chapter = scenario.Chapter with
        {
            Pages = Enumerable
                .Range(1, numberOfPages)
                .Select(id => Expression.Create() with
                {
                    Id = id
                })
                .ToArray()
        };

        scenario.Repository
            .Find(
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(ci => new Translation(
                Id: ci.ArgAt<int>(0),
                Language: Language.Bengali,
                Text: $"Text {ci.ArgAt<int>(0)}",
                Spoken: default,
                AudioId: default,
                Expression: Expression.Create() with
                {
                    Id = ci.ArgAt<int>(0)
                }));

        scenario.CorrectTranslation = new Translation(
            Id: correctId,
            Language: Language.Bengali,
            Text: $"Text {correctId}",
            Spoken: default,
            AudioId: default,
            Expression: Expression
                .Create(scenario.CorrectExpression.Level));
        
        return scenario;
    }
}