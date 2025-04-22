using System;
using System.Linq;
using Bhasha.Domain;
using NSubstitute;

namespace Bhasha.Tests.Services.Scenarios;

public static class StudyingServiceScenarioExtensions
{
    public static StudyingServiceScenario WithProfiles(this StudyingServiceScenario scenario, string userId, params ProfileKey[] profileKeys)
    {
        scenario
            .Repository
            .FindByUser(userId)
            .Returns(profileKeys
                .Select(key => new Profile(
                    Id: Random.Shared.Next(),
                    Key: key,
                    Level: 1,
                    CompletedChapters: [],
                    CurrentChapter: null))
                .ToList());

        return scenario;
    }

    public static StudyingServiceScenario WithCurrentChapter(this StudyingServiceScenario scenario, ProfileKey key, ChapterSelection currentChapter)
    {
        scenario
            .Repository
            .FindByUser(key.UserId)
            .Returns(new []
                {
                    new Profile(
                        Id: Random.Shared.Next(),
                        Key: key,
                        Level: 1,
                        CompletedChapters: [],
                        CurrentChapter: currentChapter)
                }
                .ToList());

        scenario
            .ChapterProvider
            .Load(new ChapterKey(currentChapter.ChapterId, key))
            .Returns(new DisplayedChapter(
                Id: currentChapter.ChapterId,
                Name: "Name",
                Description: "Description",
                Pages: [],
                ResourceId: null,
                StudyCards: []));
        
        return scenario;
    }
    
    public static StudyingServiceScenario WithCompletedChapters(this StudyingServiceScenario scenario, ProfileKey key, params int[] completedChapters)
    {
        scenario
            .Repository
            .FindByUser(key.UserId)
            .Returns(new []
                {
                    new Profile(
                        Id: Random.Shared.Next(),
                        Key: key,
                        Level: 1,
                        CompletedChapters: completedChapters,
                        CurrentChapter: null)
                }
                .ToList());
        
        return scenario;
    }

    public static StudyingServiceScenario WithSummaries(this StudyingServiceScenario scenario, ProfileKey key, params int[] chapterIds)
    {
        scenario
            .SummariesProvider
            .GetSummaries(Arg.Any<int>(), key.Native)
            .Returns(chapterIds
                .Select(id => new Summary(
                    ChapterId: id,
                    Name: "Name",
                    Description: "Description"))
                .ToList());
        
        return scenario;
    }

    public static StudyingServiceScenario WithValidation(this StudyingServiceScenario scenario, Validation validation)
    {
        scenario
            .Validator
            .Validate(Arg.Any<ValidationInput>())
            .Returns(validation);
        
        return scenario;
    }
}