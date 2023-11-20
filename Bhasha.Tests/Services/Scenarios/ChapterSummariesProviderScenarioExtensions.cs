using System;
using System.Linq;
using Bhasha.Shared.Domain;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public static class ChapterSummariesProviderScenarioExtensions
{
    public static ChapterSummariesProviderScenario WithChapters(this ChapterSummariesProviderScenario scenario, int level, params Guid[] chapterIds)
    {
        scenario
            .ChapterRepository
            .FindByLevel(Arg.Any<int>())
            .Returns(
                chapterIds
                    .Select(chapterId => new Chapter(
                        Id: chapterId,
                        RequiredLevel: level,
                        NameId: scenario.ExpressionIds["Name"],
                        DescriptionId: scenario.ExpressionIds["Description"],
                        Pages: new []
                        {
                            new Page(
                                PageType.MultipleChoice,
                                ExpressionId: scenario.ExpressionIds["FirstPage"]),
                            new Page(
                                PageType.MultipleChoice,
                                ExpressionId: scenario.ExpressionIds["SecondPage"])
                        },
                        ResourceId: default,
                        AuthorId: "UUID-125632-2415-3453"))
                    .ToAsyncEnumerable());
        
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
                    Id: Guid.NewGuid(),
                    ExpressionId: expressionId,
                    Language: language,
                    Text: expression,
                    Spoken: default,
                    AudioId: default
                ));
        }
        
        return scenario;
    }
}