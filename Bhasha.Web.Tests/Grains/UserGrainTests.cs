using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Grains;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Pages.Student;
using Bhasha.Web.Tests.Support;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Xunit;

namespace Bhasha.Web.Tests.Grains;

public class UserGrainTests : TestKitBase
{
    private readonly IProfileRepository _repository;

    public UserGrainTests()
	{
        _repository = Substitute.For<IProfileRepository>();
        _repository
            .GetProfiles("user-123")
            .Returns(Array.Empty<Profile>().ToAsyncEnumerable());

        Silo.AddService(_repository);
    }

    [Theory, AutoData]
    public async Task GivenUserProfileForLanguages_WhenGetProfile_ThenReturnProfile(Profile profile)
    {
        // setup
        _repository
            .GetProfiles("user-123")
            .Returns(new[] { profile }.ToAsyncEnumerable());

        // act
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");
        var result = await grain.GetProfile(profile.Key.LangId);

        // verify
        result.Should().Be(profile);
    }

    [Fact]
    public async Task GivenNoUserProfile_WhenGetProfile_ThenThrowException()
    {
        // setup
        _repository
            .GetProfiles("user-123")
            .Returns(Array.Empty<Profile>().ToAsyncEnumerable());

        // act
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");
        var exception = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await grain.GetProfile(SupportedLanguageKey.Create()));

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenUserProfileWithoutChapterId_WhenGetCurrentChapter_ThenReturnNull(Profile profile)
    {
        // setup
        _repository
            .GetProfiles("user-123")
            .Returns(new[] { profile with { CurrentChapter = null } }.ToAsyncEnumerable());

        // act
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");
        var result = await grain.GetCurrentChapter(profile.Key.LangId);

        // verify
        result.Should().BeNull();
    }

    [Theory, AutoData]
    public async Task GivenUserProfileWithChapterId_WhenGetCurrentChapter_ThenReturnDisplayedChapter(Profile profile, DisplayedChapter chapter, ChapterKey chapterKey)
    {
        // setup
        _repository
            .GetProfiles("user-123")
            .Returns(new[]
            {
                profile with
                {
                    CurrentChapter = new ChapterSelection(chapterKey.ChapterId, 0, Array.Empty<ValidationResultType>()),
                    Key = profile.Key with { LangId = chapterKey.LangId }
                }
            }.ToAsyncEnumerable());

        var chapterGrain = Silo.AddProbe<IDisplayChapterGrain>(chapterKey.ToString());
        chapterGrain
            .Setup(grain => grain.Display())
            .Returns(new ValueTask<DisplayedChapter>(chapter));

        // act
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");
        var result = await grain.GetCurrentChapter(chapterKey.LangId);

        // verify
        result.Should().Be(chapter);
    }

    [Theory, AutoData]
    public async Task GivenUserProfile_WhenGetSummaries_ThenReturnSummariesOfUncompletedChapters(Profile profile, Summary summary)
    {
        // setup
        _repository
            .GetProfiles("user-123")
            .Returns(new[] { profile with { CurrentChapter = null } }.ToAsyncEnumerable());

        var expectedKey = new SummaryCollectionKey(profile.Level, profile.Key.LangId);
        var chapterGrain = Silo.AddProbe<ISummaryGrain>(expectedKey.ToString());
        chapterGrain
            .Setup(grain => grain.GetSummaries())
            .Returns(new ValueTask<ImmutableList<Summary>>(
                new[] {
                summary,
                summary with { ChapterId = profile.CompletedChapters.First() }
            }.ToImmutableList()));

        // act
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");
        var result = await grain.GetSummaries(profile.Key.LangId);

        // verify
        result.Should().BeEquivalentTo(new[] {summary});
    }

    [Fact]
    public async Task GivenProfileDoesntExist_WhenCreateProfile_ThenAddProfileToRepository()
    {
        // setup
        var languages = new LangKey(Language.English, Language.Bengali);

        _repository
            .GetProfiles("user-123")
            .Returns(Array.Empty<Profile>().ToAsyncEnumerable());

        // act
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");
        await grain.CreateProfile(languages);

        // verify
        var expectedProfile = Profile.Empty(new ProfileKey("user-123", languages));
        await _repository.Received(1).Add(expectedProfile);
    }

    [Fact]
    public async Task GivenSameNativeAndTargetLanguageSelected_WhenCreateProfile_ThenThrow()
    {
        // setup
        var languages = new LangKey(Language.English, Language.English);
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");

        // act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await grain.CreateProfile(languages));

        // verify
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenNativeLanguageNotSupported_WhenCreateProfile_ThenThrow()
    {
        // setup
        var languages = new LangKey("test", Language.English);
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");

        // act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await grain.CreateProfile(languages));

        // verify
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenTargetLanguageNotSupported_WhenCreateProfile_ThenThrow()
    {
        // setup
        var languages = new LangKey(Language.English, "test");
        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");

        // act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await grain.CreateProfile(languages));

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenProfileAlreadyExists_WhenCreateProfile_ThenThrow(Profile profile)
    {
        // setup
        var languages = new LangKey(Language.English, Language.Bengali);

        _repository
            .GetProfiles("user-123")
            .Returns(new[]
            {
                profile with
                {
                    Key = profile.Key with
                    {
                        LangId = languages
                    }
                }
            }.ToAsyncEnumerable());

        var grain = await Silo.CreateGrainAsync<UserGrain>("user-123");

        // act
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await grain.CreateProfile(languages));

        // verify
        exception.Should().NotBeNull();
    }
}

