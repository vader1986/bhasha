using System;
using Bhasha.Web.Domain;
using Bhasha.Web.Services;
using Xunit;

namespace Bhasha.Web.Tests.Services;

public class ProfileFactoryTests
{
    [Fact]
    public void GivenAProfileFactory_WhenCreate_ThenReturnNewProfile()
    {
        // prepare
        var factory = new ProfileFactory();

        // act
        var profile = factory.Create();

        // verify
        Assert.Equal(profile.Id, Guid.Empty);
        Assert.Equal(profile.UserId, string.Empty);
        Assert.Equal(profile.Languages, LangKey.Unknown);
        Assert.Equal(1, profile.Progress.Level);
        Assert.Equal(profile.Progress.ChapterId, Guid.Empty);
        Assert.Equal(profile.Progress.CompletedChapters, Array.Empty<Guid>());
        Assert.Equal(0, profile.Progress.PageIndex);
        Assert.Equal(profile.Progress.Pages, Array.Empty<ValidationResultType>());
    }
}

