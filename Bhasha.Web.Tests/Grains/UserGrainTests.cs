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
    private readonly IProfileLookup _profileLookup;

    public UserGrainTests()
	{
        _profileLookup = Substitute.For<IProfileLookup>();

        Silo.AddService(_profileLookup);
    }

    [Theory, AutoData]
    public async Task GivenUserProfileForLanguages_WhenGetProfile_ThenReturnProfile(Profile profile)
    {
        // setup
        _profileLookup
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
        _profileLookup
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
        _profileLookup
            .GetProfiles("user-123")
            .Returns(new[] { profile with { ChapterId = null } }.ToAsyncEnumerable());

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
        _profileLookup
            .GetProfiles("user-123")
            .Returns(new[]
            {
                profile with
                {
                    ChapterId = chapterKey.ChapterId,
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
        _profileLookup
            .GetProfiles("user-123")
            .Returns(new[] { profile with { ChapterId = null } }.ToAsyncEnumerable());

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
}

