using System;
using System.Linq;
using System.Threading;
using Bhasha.Domain;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public static class ChapterSummariesProviderScenarioExtensions
{
    public static ChapterSummariesProviderScenario WithChapters(this ChapterSummariesProviderScenario scenario, int level, params int[] chapterIds)
    {
        scenario
            .ChapterRepository
            .FindByLevel(level: Arg.Any<int>(), token: Arg.Any<CancellationToken>())
            .Returns(
                returnThis: chapterIds
                    .Select(selector: chapterId => new Chapter(
                        Id: chapterId,
                        RequiredLevel: level,
                        Name: Expression.Create() with { Id = scenario.ExpressionIds[key: "Name"] },
                        Description: Expression.Create() with { Id = scenario.ExpressionIds[key: "Description"] },
                        Pages:
                        [
                            Expression.Create() with { Id = scenario.ExpressionIds[key: "FirstPage"] },
                            Expression.Create() with { Id = scenario.ExpressionIds[key: "SecondPage"] }
                        ],
                        ResourceId: default,
                        AuthorId: "UUID-125632-2415-3453",
                        StudyCards: []))
                    .ToList());
        
        return scenario;
    }

    public static ChapterSummariesProviderScenario WithTranslations(this ChapterSummariesProviderScenario scenario, Language language)
    {
        foreach (var (expression, expressionId) in scenario.ExpressionIds)
        {
            scenario
                .TranslationProvider
                .Find(expressionId, language)
                .Returns(new Translation(
                    Id: Random.Shared.Next(),
                    Expression: Expression.Create() with { Id = expressionId },
                    Language: language,
                    Text: expression,
                    Spoken: null,
                    AudioId: null
                ));
        }
        
        return scenario;
    }
}