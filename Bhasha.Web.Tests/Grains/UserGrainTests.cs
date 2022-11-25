using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Grains;
using Bhasha.Web.Interfaces;
using FluentAssertions;
using NSubstitute;
using Orleans.TestKit;
using Xunit;

namespace Bhasha.Web.Tests.Grains;

public class UserGrainTests : TestKitBase
{
    private readonly IRepository<Profile> _profileRepository;


    public UserGrainTests()
	{
        _profileRepository = Substitute.For<IRepository<Profile>>();

        Silo.AddService(_profileRepository);
    }

    [Theory, AutoData]
    public async Task GivenProfileInRepository_WhenGetProfile_ThenReturnProfile(Profile profile)
    {
        // setup
        _profileRepository
            .Find(default!)
            .ReturnsForAnyArgs(new[] { profile });

        // act
        var grain = await Silo.CreateGrainAsync<UserGrain>("user");
        var result = await grain.GetProfile(profile.Key.LangId);

        // verify
        result.Should().Be(profile);
    }
}

