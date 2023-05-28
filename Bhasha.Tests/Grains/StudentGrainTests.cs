using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Bhasha.Grains;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Orleans.TestKit.Streams;
using Xunit;

namespace Bhasha.Tests.Grains;

public class StudentGrainTests : TestKitBase
{
    private readonly IProfileRepository _repository = Substitute.For<IProfileRepository>();
    private readonly IValidator _validator = Substitute.For<IValidator>();
    private readonly IChapterSummariesProvider _summariesProvider = Substitute.For<IChapterSummariesProvider>();
    private readonly TestStream<Profile> _stream;

    public StudentGrainTests()
	{
        Silo.AddService(_repository);
        Silo.AddService(_validator);
        Silo.AddService(_summariesProvider);

        _stream = Silo.AddStreamProbe<Profile>();
    }

    #region GetProfile

    [Theory, AutoData]
    public async Task GivenGetProfileRequest_WhenProfileInRepository_ThenReturnProfile(string userId, Profile[] profiles)
    {
        // setup
        _repository
            .FindByUser(userId)
            .Returns(profiles.ToAsyncEnumerable());

        var profile = profiles.Last();
        var profileKey = profile.Key;

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var result = await grain.GetProfile(profileKey.LangId);

        // verify
        result.Should().Be(profile);
    }

    [Theory, AutoData]
    public async Task GivenGetProfileRequest_WhenProfileNotInRepository_ThenThrowException(string userId)
    {
        // setup
        _repository
            .FindByUser(userId)
            .Returns(Enumerable.Empty<Profile>().ToAsyncEnumerable());

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await grain.GetProfile(new LangKey(Language.English, Language.Bengali)));

        // verify
        exception.Should().NotBeNull();
    }

    #endregion

    #region GetProfiles

    [Theory, AutoData]
    public async Task GivenProfilesInRepository_WhenGetProfiles_ThenReturnProfilesFromRepository(string userId)
    {
        // setup
        var profiles = new[]
        {
            new Profile(Guid.NewGuid(), new ProfileKey(userId, new LangKey(Language.English, Language.Bengali)), 1, Array.Empty<Guid>(), default),
            new Profile(Guid.NewGuid(), new ProfileKey(userId, new LangKey(Language.Bengali, Language.English)), 3, Array.Empty<Guid>(), default)
        };

        _repository
            .FindByUser(userId)
            .Returns(profiles.ToAsyncEnumerable());

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var result = await grain.GetProfiles();

        // verify
        result.Should().BeEquivalentTo(profiles);
    }

    #endregion

    #region CreateProfile

    [Theory, AutoData]
    public async Task GivenExistingProfile_WhenCreateProfileForSameLanguageKey_ThenThrowException(string userId)
    {
        // setup
        var languageKey = new LangKey(Language.English, Language.Bengali);
        var profiles = new[]
        {
            new Profile(Guid.NewGuid(), new ProfileKey(userId, languageKey), 1, Array.Empty<Guid>(), default),
        };

        _repository
            .FindByUser(userId)
            .Returns(profiles.ToAsyncEnumerable());

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var exception = Assert.ThrowsAsync<ArgumentException>(
            async () => await grain.CreateProfile(languageKey));

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenUserGrain_WhenCreateProfileWithSameNativeAndTargetLanguage_ThenThrowException(string userId)
    {
        // setup
        var languageKey = new LangKey(Language.Bengali, Language.Bengali);
        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var exception = Assert.ThrowsAsync<ArgumentException>(
            async () => await grain.CreateProfile(languageKey));

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenUserGrain_WhenCreateProfileWithSameUnsupportedNativeLanguage_ThenThrowException(string userId)
    {
        // setup
        var languageKey = new LangKey(Language.Unknown, Language.Bengali);
        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var exception = Assert.ThrowsAsync<ArgumentException>(
            async () => await grain.CreateProfile(languageKey));

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenUserGrain_WhenCreateProfileWithSameUnsupportedTargetLanguage_ThenThrowException(string userId)
    {
        // setup
        var languageKey = new LangKey(Language.English, Language.Unknown);
        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var exception = Assert.ThrowsAsync<ArgumentException>(
            async () => await grain.CreateProfile(languageKey));

        // verify
        exception.Should().NotBeNull();
    }

    [Theory, AutoData]
    public async Task GivenUserGrain_WhenCreateNewProfile_ThenReturnProfile(string userId, Profile profile)
    {
        // setup
        var languageKey = new LangKey(Language.English, Language.Bengali);
        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        _repository
            .Add(Profile.Empty(new ProfileKey(userId, languageKey)))
            .Returns(profile);

        // act
        var result = await grain.CreateProfile(languageKey);

        // verify
        result.Should().Be(profile);
    }

    #endregion

    #region GetCurrentChapter

    [Theory, AutoData]
    public async Task GivenProfileWithoutCurrentChapter_WhenGetCurrentChapter_ThenReturnNull(string userId)
    {
        // setup
        var languageKey = new LangKey(Language.English, Language.Bengali);
        var profiles = new[]
        {
            new Profile(Guid.NewGuid(), new ProfileKey(userId, languageKey), 1, Array.Empty<Guid>(), CurrentChapter: null)
        };

        _repository
            .FindByUser(userId)
            .Returns(profiles.ToAsyncEnumerable());

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var result = await grain.GetCurrentChapter(languageKey);

        // verify
        result.Should().BeNull();
    }

    [Theory, AutoData]
    public async Task GivenProfileWithCurrentChapter_WhenGetCurrentChapter_ThenReturnDisplayedChapter(string userId, DisplayedChapter chapter)
    {
        // setup
        var chapterId = Guid.NewGuid();
        var chapterSelection = new ChapterSelection(chapterId, 0, new[] { ValidationResult.Wrong });

        var languageKey = new LangKey(Language.English, Language.Bengali);
        var profiles = new[]
        {
            new Profile(Guid.NewGuid(), new ProfileKey(userId, languageKey), 1, Array.Empty<Guid>(), CurrentChapter: chapterSelection)
        };

        var probe = Silo.AddProbe<IDisplayChapterGrain>(new ChapterKey(chapterId, languageKey));

        probe
            .Setup(g => g.Display())
            .Returns(Task.FromResult(chapter));

        _repository
            .FindByUser(userId)
            .Returns(profiles.ToAsyncEnumerable());

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var result = await grain.GetCurrentChapter(languageKey);

        // verify
        result.Should().Be(chapter);
    }

    #endregion

    #region GetSummaries

    [Theory, AutoData]
    public async Task GivenLanguageKey_WhenGetSummaries_ThenReturnSummariesFromGrain(string userId, Summary[] summaries)
    {
        // setup
        var languageKey = new LangKey(Language.English, Language.Bengali);
        var profiles = new[]
        {
            new Profile(Guid.NewGuid(), new ProfileKey(userId, languageKey), Level: 5, Array.Empty<Guid>(), default)
        };

        _repository
            .FindByUser(userId)
            .Returns(profiles.ToAsyncEnumerable());

        _summariesProvider
            .GetSummaries(5, languageKey)
            .Returns(summaries);

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var result = await grain.GetSummaries(languageKey);

        // verify
        var expectedChapters = summaries.Select(x => new DisplayedSummary(x.ChapterId, x.Name, x.Description, false));
        result.Should().BeEquivalentTo(expectedChapters);
    }

    #endregion

    #region SelectChapter

    [Theory, AutoData]
    public async Task GivenProfile_WhenSelectChapter_ThenReturnChapterFromGrain(string userId, DisplayedChapter chapter)
    {
        // setup
        var languageKey = new LangKey(Language.English, Language.Bengali);
        var profiles = new[]
        {
            new Profile(Guid.NewGuid(), new ProfileKey(userId, languageKey), Level: 5, Array.Empty<Guid>(), default)
        };

        _repository
            .FindByUser(userId)
            .Returns(profiles.ToAsyncEnumerable());

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);
        var key = new ChapterKey(chapter.Id, languageKey);
        var probe = Silo.AddProbe<IDisplayChapterGrain>(key);

        probe
            .Setup(g => g.Display())
            .Returns(Task.FromResult(chapter));

        // act
        var result = await grain.SelectChapter(chapter.Id, languageKey);

        // verify
        result.Should().Be(chapter);
    }

    #endregion

    #region Submit

    [Theory, AutoData]
    public async Task GivenProfile_WhenSubmitSolution_ThenReturnValidationResult(string userId, ValidationInput input)
    {
        // setup
        var profiles = new[]
        {
            new Profile(Guid.NewGuid(), new ProfileKey(userId, input.Languages), Level: 5, Array.Empty<Guid>(), default)
        };

        _repository
            .FindByUser(userId)
            .Returns(profiles.ToAsyncEnumerable());

        _validator
            .Validate(input)
            .Returns(new Validation(ValidationResult.Correct));

        var grain = await Silo.CreateGrainAsync<StudentGrain>(userId);

        // act
        var result = await grain.Submit(input);

        // verify
        result.Should().Be(new Validation(ValidationResult.Correct));
    }

    #endregion
}

