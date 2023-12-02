using System;
using System.Linq;
using System.Threading;
using Bhasha.Shared.Domain;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public static class ChapterSummariesProviderScenarioExtensions
{
    public static ChapterSummariesProviderScenario WithChapters(this ChapterSummariesProviderScenario scenario, int level, params int[] chapterIds)
    {
        scenario
            .ChapterRepository
            .FindByLevel(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                chapterIds
                    .Select(chapterId => new Chapter(
                        Id: chapterId,
                        RequiredLevel: level,
                        Name: Expression.Create() with { Id = scenario.ExpressionIds["Name"] },
                        Description: Expression.Create() with { Id = scenario.ExpressionIds["Description"] },
                        Pages: new []
                        {
                            Expression.Create() with { Id = scenario.ExpressionIds["FirstPage"] },
                            Expression.Create() with { Id = scenario.ExpressionIds["SecondPage"] },
                        },
                        ResourceId: default,
                        AuthorId: "UUID-125632-2415-3453"))
                    .ToList());
        
        return scenario;
    }

    public static ChapterSummariesProviderScenario WithTranslations(this ChapterSummariesProviderScenario scenario, Language language)
    {
        foreach (var (expression, expressionId) in scenario.ExpressionIds)
        {
            scenario
                .TranslationRepository
                .Find(expressionId, language)
                .Returns(new Translation(
                    Id: Random.Shared.Next(),
                    Expression: Expression.Create() with { Id = expressionId },
                    Language: language,
                    Text: expression,
                    Spoken: default,
                    AudioId: default
                ));
        }
        
        return scenario;
    }
}