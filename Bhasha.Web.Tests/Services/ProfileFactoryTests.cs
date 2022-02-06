using System;
using Bhasha.Web.Domain;
using Bhasha.Web.Services;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services;

public class ProfileFactoryTests
{
    [Test]
    public void GivenAProfileFactory_WhenCreate_ThenReturnNewProfile()
    {
        // prepare
        var factory = new ProfileFactory();

        // act
        var profile = factory.Create();

        // verify
        Assert.That(profile.Id, Is.EqualTo(Guid.Empty));
        Assert.That(profile.UserId, Is.EqualTo(string.Empty));
        Assert.That(profile.Native, Is.EqualTo(Language.Unknown.ToString()));
        Assert.That(profile.Target, Is.EqualTo(Language.Unknown.ToString()));
        Assert.That(profile.Progress.Level, Is.EqualTo(1));
        Assert.That(profile.Progress.ChapterId, Is.EqualTo(Guid.Empty));
        Assert.That(profile.Progress.CompletedChapters, Is.EqualTo(Array.Empty<Guid>()));
        Assert.That(profile.Progress.PageIndex, Is.EqualTo(0));
        Assert.That(profile.Progress.CompletedPages, Is.EqualTo(Array.Empty<int>()));
    }
}

