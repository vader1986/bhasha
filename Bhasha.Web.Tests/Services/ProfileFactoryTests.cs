using System;
using Bhasha.Web.Services;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services;

[TestFixture]
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
        Assert.That(profile.Level, Is.EqualTo(0));
        Assert.That(profile.CompletedChapters, Is.EqualTo(0));
    }
}

