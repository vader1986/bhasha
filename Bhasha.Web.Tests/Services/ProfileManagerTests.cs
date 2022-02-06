using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services;

public class ProfileManagerTests
{
    private ProfileManager _profileManager = default!;
    private IRepository<Profile> _repository = default!;
    private IFactory<Profile> _factory = default!;

    [SetUp]
    public void Before()
    {
        _repository = A.Fake<IRepository<Profile>>();
        _factory = A.Fake<IFactory<Profile>>();
        _profileManager = new ProfileManager(_repository, _factory);

        var progress = new Progress(1, Guid.Empty, new Guid[0], 0, new int[0]);
        var profile = new Profile(Guid.NewGuid(), "user-123", Language.English, Language.Bengali, progress);

        A.CallTo(() => _factory.Create()).Returns(profile);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void GivenCreateCall_WhenUserIdIsNullOrEmpty_ThenThrowException(string emptyUserId)
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _profileManager.Create(emptyUserId, Language.English, Language.Bengali));
    }

    [Test]
    public void GivenCreateCall_WhenNativeLanguageNotSupported_ThenThrowException()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _profileManager.Create("user-123", Language.Unknown, Language.Bengali));
    }

    [Test]
    public void GivenCreateCall_WhenTargetLanguageNotSupported_ThenThrowException()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _profileManager.Create("user-123", Language.Bengali, Language.Unknown));
    }

    [Test]
    public void GivenCreateCall_WhenTargetAndNativeLanguagesAreEqual_ThenThrowException()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _profileManager.Create("user-123", Language.Bengali, Language.Bengali));
    }

    [Test, AutoData]
    public void GivenCreateCall_WhenProfileAlreadyExists_ThenThrowException(Profile profile)
    {
        profile = profile with { UserId = "user-123", Native = Language.English, Target = Language.Bengali };

        A.CallTo(() => _repository.Find(A<Expression<Func<Profile, bool>>>.Ignored))
            .Returns(Task.FromResult(new[] { profile }));

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _profileManager.Create(profile.UserId, profile.Native, profile.Target));
    }

    [Test]
    public async Task GivenCreateCall_WhenCreated_ThenReturnNewProfile()
    {
        // prepare
        A.CallTo(() => _repository.Find(A<Expression<Func<Profile, bool>>>.Ignored))
            .Returns(Task.FromResult(Array.Empty<Profile>()));

        // act
        var profile = await _profileManager.Create("user-123", Language.English, Language.Bengali);

        // verify
        Assert.That(profile, Is.Not.Null);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void GivenEmptyUserId_WhenGetProfiles_ThenThrowException(string emptyUserId)
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _profileManager.GetProfiles(emptyUserId));
    }

    [Test, AutoData]
    public async Task GivenUserProfiles_WhenGetProfilesForUser_ThenReturnProfiles(Profile[] profiles)
    {
        // prepare
        A.CallTo(() => _repository.Find(A<Expression<Func<Profile, bool>>>.Ignored))
            .Returns(Task.FromResult(profiles));

        // act
        var result = await _profileManager.GetProfiles("user-123");

        // verify
        Assert.AreEqual(profiles, result);
    }
}