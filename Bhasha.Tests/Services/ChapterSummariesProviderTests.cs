using AutoFixture.Xunit2;
using System;
using System.Threading.Tasks;
using Bhasha.Domain;
using Bhasha.Tests.Services.Scenarios;
using Bhasha.Tests.Support;
using Xunit;

namespace Bhasha.Tests.Services;

public class ChapterSummariesProviderTests
{
    [Theory, AutoData]
    public async Task GetSummariesForChaptersAndTranslations(int level)
    {
        // setup
        var chapterOneId = 1;
        var chapterTwoId = 2;
        
        var profileKey = SupportedProfileKey
            .Create();
        
        var scenario = new ChapterSummariesProviderScenario()
            .WithChapters(level, chapterOneId, chapterTwoId)
            .WithTranslations(profileKey.Native);

        // act
        var result = await scenario.Sut.GetSummaries(level, profileKey.Native);

        // verify
        Assert.Equal([
            new Summary(chapterOneId, "Name", "Description"),
            new Summary(chapterTwoId, "Name", "Description")
        ], result);
    }

    [Theory, AutoData]
    public async Task GetSummariesForMissingTranslations(int level, int chapterId)
    {
        // setup
        var profileKey = SupportedProfileKey
            .Create();
        
        var scenario = new ChapterSummariesProviderScenario()
            .WithChapters(level, chapterId);

        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await scenario.Sut.GetSummaries(level, profileKey.Native));

        // verify
        Assert.NotNull(exception);
    }
}

