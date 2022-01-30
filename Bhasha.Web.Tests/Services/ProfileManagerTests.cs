using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;
using Bhasha.Web.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services;

[TestFixture]
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

        A.CallTo(() => _factory.Create()).Returns(
            new Profile(Guid.NewGuid(), "user-123", Language.English, Language.Bengali, 0, 0));
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

    [Test]
    public void GivenCreateCall_WhenProfileAlreadyExists_ThenThrowException()
    {
        var existingProfile = new Profile(Guid.Empty, "user-123", Language.English, Language.Bengali, 1, 1);
        A.CallTo(() => _repository.Find(A<Expression<Func<Profile, bool>>>.Ignored))
            .Returns(Task.FromResult(new[] { existingProfile }));

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _profileManager.Create("user-123", Language.English, Language.Bengali));
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
}