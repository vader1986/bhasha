using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using NSubstitute;
using Xunit;

namespace Bhasha.Web.Tests.Services;

public class ProfileManagerTests
{
    private readonly ProfileManager _profileManager;
    private readonly IRepository<Profile> _repository;

    public ProfileManagerTests()
    {
        _repository = Substitute.For<IRepository<Profile>>();
        _profileManager = new ProfileManager(_repository);
    }

    [Fact]
    public void GivenCreateCall_WhenUserIdIsNullOrEmpty_ThenThrowException()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _profileManager.Create(null!, new LangKey(Language.English, Language.Bengali)));
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _profileManager.Create("", new LangKey(Language.English, Language.Bengali)));
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _profileManager.Create(" ", new LangKey(Language.English, Language.Bengali)));
    }

    [Fact]
    public void GivenCreateCall_WhenNativeLanguageNotSupported_ThenThrowException()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _profileManager.Create("user-123", new LangKey(Language.Unknown, Language.Bengali)));
    }

    [Fact]
    public void GivenCreateCall_WhenTargetLanguageNotSupported_ThenThrowException()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _profileManager.Create("user-123", new LangKey(Language.Bengali, Language.Unknown)));
    }

    [Fact]
    public void GivenCreateCall_WhenTargetAndNativeLanguagesAreEqual_ThenThrowException()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _profileManager.Create("user-123", new LangKey(Language.Bengali, Language.Bengali)));
    }

    [Theory, AutoData]
    public void GivenCreateCall_WhenProfileAlreadyExists_ThenThrowException(Profile profile)
    {
        profile = profile with
        {
            Key = new ProfileKey("user-123", new LangKey(Language.English, Language.Bengali))
        };

        _repository.Find(default!).ReturnsForAnyArgs(new[] { profile });

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _profileManager.Create(profile.Key.UserId, new LangKey(profile.Key.LangId.Native, profile.Key.LangId.Target)));
    }

    [Fact]
    public async Task GivenCreateCall_WhenCreated_ThenAddNewProfile()
    {
        // prepare
        _repository.Find(default!).ReturnsForAnyArgs(Array.Empty<Profile>());

        // act
        await _profileManager.Create("user-123", new LangKey(Language.English, Language.Bengali));

        // verify
        await _repository.Received(1).Add(Arg.Any<Profile>());
    }

    [Fact]
    public void GivenEmptyUserId_WhenGetProfiles_ThenThrowException()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _profileManager.GetProfiles(null!));
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _profileManager.GetProfiles(""));
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _profileManager.GetProfiles(" "));
    }

    [Theory, AutoData]
    public async Task GivenUserProfiles_WhenGetProfilesForUser_ThenReturnProfiles(Profile[] profiles)
    {
        // prepare
        _repository.Find(default!).ReturnsForAnyArgs(profiles);

        // act
        var result = await _profileManager.GetProfiles("user-123");

        // verify
        Assert.Equal(profiles, result);
    }
}