using AutoFixture.Xunit2;
using System;
using System.Threading.Tasks;
using Bhasha.Shared.Domain;
using Bhasha.Tests.Services.Scenarios;
using Bhasha.Tests.Support;
using Xunit;
using FluentAssertions;

namespace Bhasha.Tests.Services;

public class ChapterSummariesProviderTests
{
    [Theory, AutoData]
    public async Task GetSummariesForChaptersAndTranslations(int level, Guid chapterOneId, Guid chapterTwoId)
    {
        // setup
        var profileKey = SupportedProfileKey
            .Create();
        
        var scenario = new ChapterSummariesProviderScenario()
            .WithChapters(level, chapterOneId, chapterTwoId)
            .WithTranslations(profileKey.Native);

        // act
        var result = await scenario.Sut.GetSummaries(level, profileKey);

        // verify
        result
            .Should()
            .BeEquivalentTo(new[]
            {
                new Summary(chapterOneId, "Name", "Description"),
                new Summary(chapterTwoId, "Name", "Description")
            });
    }

    [Theory, AutoData]
    public async Task GetSummariesForMissingTranslations(int level, Guid chapterId)
    {
        // setup
        var profileKey = SupportedProfileKey
            .Create();
        
        var scenario = new ChapterSummariesProviderScenario()
            .WithChapters(level, chapterId);

        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await scenario.Sut.GetSummaries(level, profileKey));

        // verify
        exception
            .Should()
            .NotBeNull();
    }
}

