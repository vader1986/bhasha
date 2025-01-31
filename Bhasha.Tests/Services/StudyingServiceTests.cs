using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Tests.Services.Scenarios;
using Bhasha.Tests.Support;
using NSubstitute;
using Xunit;

namespace Bhasha.Tests.Services;

public class StudyingServiceTests
{
    [Theory, AutoData]
    public async Task GetProfileForNonExisingKey(ProfileKey key)
    {
        // arrange
        var scenario = new StudyingServiceScenario();
        
        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await scenario.Sut.GetProfile(key));

        // assert
        Assert.NotNull(exception);
    }

    [Theory, AutoData]
    public async Task GetExistingProfile(ProfileKey key)
    {
        // arrange
        var scenario = new StudyingServiceScenario()
            .WithProfiles(key.UserId, key);
        
        // act
        var result = await scenario.Sut.GetProfile(key);
        
        // assert
        Assert.NotNull(result);
    }
    
    [Theory, AutoData]
    public async Task GetProfiles(string userId, ProfileKey[] keys)
    {
        // arrange
        var scenario = new StudyingServiceScenario()
            .WithProfiles(userId, keys);
        
        // act
        var result = await scenario.Sut.GetProfiles(userId);
        
        // assert
        Assert.Equal(keys.Length, result.Count);
    }

    [Fact]
    public async Task CreateProfileWithSameNativeAndTargetLanguage()
    {
        // arrange
        var key = SupportedProfileKey.Create();
        var scenario = new StudyingServiceScenario();
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await scenario.Sut.CreateProfile(key with
            {
                Native = key.Target
            }));
        
        // assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async Task CreateProfileWithUnsupportedNativeLanguage()
    {
        // arrange
        var key = SupportedProfileKey.Create() with { Native = "unknown " };
        var scenario = new StudyingServiceScenario();
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await scenario.Sut.CreateProfile(key));
        
        // assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async Task CreateProfileWithUnsupportedTargetLanguage()
    {
        // arrange
        var key = SupportedProfileKey.Create() with { Target = "unknown " };
        var scenario = new StudyingServiceScenario();
        
        // act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await scenario.Sut.CreateProfile(key));
        
        // assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateValidProfile()
    {
        // arrange
        var key = SupportedProfileKey.Create();
        var scenario = new StudyingServiceScenario();
        
        // act
        await scenario.Sut.CreateProfile(key);
        
        // assert
        await scenario.Repository
            .Received(1)
            .Add(new Profile(
                Id: default,
                Key: key,
                Level: 1,
                CompletedChapters: Array.Empty<int>(),
                CurrentChapter: null));
    }

    [Theory, AutoData]
    public async Task GetCurrentChapterForProfileWithCurrentChapter(ChapterSelection selection)
    {
        // arrange
        var key = SupportedProfileKey.Create();
        var scenario = new StudyingServiceScenario()
            .WithCurrentChapter(key, selection);
        
        // act
        var result = await scenario.Sut.GetCurrentChapter(key);
        
        // assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCurrentChapterForProfileWithoutCurrentChapter()
    {
        // arrange
        var key = SupportedProfileKey.Create();
        var scenario = new StudyingServiceScenario()
            .WithProfiles(key.UserId, key);

        // act
        var result = await scenario.Sut.GetCurrentChapter(key);
        
        // assert
        Assert.Null(result);
    }

    [Theory, AutoData]
    public async Task GetSummariesWithCompletedAndUncompletedChapters(int[] chapterIds)
    {
        // arrange
        var key = SupportedProfileKey
            .Create();
        
        var completedChapters = chapterIds
            .TakeEverySecond()
            .ToArray();
        
        var scenario = new StudyingServiceScenario()
            .WithCompletedChapters(key, completedChapters)
            .WithSummaries(key, chapterIds);
        
        // act
        var result = await scenario.Sut.GetSummaries(key);
        
        // assert
        Assert.All(result
            .TakeEverySecond(), x => Assert.True(x.Completed));

        Assert.All(result
            .TakeEverySecond(offset: 1), x => Assert.False(x.Completed));
    }
}