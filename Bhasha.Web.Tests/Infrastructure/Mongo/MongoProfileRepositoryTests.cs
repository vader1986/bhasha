using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Domain;
using Bhasha.Web.Infrastructure.Mongo;
using FluentAssertions;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace Bhasha.Web.Tests.Infrastructure.Mongo;

public class MongoProfileRepositoryTests : IDisposable
{
    private const string DbName = "TestDB";
    private readonly MongoDbRunner _runner;
    private readonly MongoProfileRepository _repository;
    private readonly MongoClient _client;

    public MongoProfileRepositoryTests()
    {
        _runner = MongoDbRunner.Start();
        _client = new(_runner.ConnectionString);
        _repository = new MongoProfileRepository(_client, new MongoSettings
        {
            DatabaseName = DbName
        });
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _runner.Dispose();
    }

    #region Add

    [Theory, AutoData]
    public async Task GivenProfileWithEmptyId_WhenAdd_ThenReturnProfileWithNewId(Profile profile)
    {
        // act
        var result = await _repository.Add(profile with { Id = Guid.Empty });

        // verify
        result.Id.Should().NotBe(Guid.Empty);
    }

    [Theory, AutoData]
    public async Task GivenProfileWithId_WhenAdd_ThenReturnProfile(Profile profile)
    {
        // act
        var result = await _repository.Add(profile);

        // verify
        result.Should().Be(profile);
    }

    #endregion

    #region Update

    [Theory, AutoData]
    public async Task GivenProfile_WhenUpdate_ThenUpdateProfileInDatabase(Profile profile, Profile updatedProfile)
    {
        // setup
        await _client
            .GetDatabase(DbName)
            .GetCollection<Profile>("Profile")
            .InsertOneAsync(profile);

        // act
        await _repository.Update(updatedProfile with { Id = profile.Id });

        // verify
        var result = await _client
            .GetDatabase(DbName)
            .GetCollection<Profile>("Profile")
            .AsQueryable()
            .SingleAsync();

        result.Should().Be(updatedProfile with { Id = profile.Id });
    }

    #endregion

    #region FindByUser

    [Theory, AutoData]
    public async Task GivenProfiles_WhenFindByUser_ThenReturnProfilesForUser(Profile profile)
    {
        // setup
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        await _client
            .GetDatabase(DbName)
            .GetCollection<Profile>("Profile")
            .InsertManyAsync(new[]
            {
                profile with { Id = ids[0], Key = profile.Key with { UserId = "user-1" } },
                profile with { Id = ids[1], Key = profile.Key with { UserId = "user-2" } },
                profile with { Id = ids[2], Key = profile.Key with { UserId = "user-1" } },
            });

        // act
        var result = await _repository.FindByUser("user-1").ToListAsync();

        // verify
        result.Should().BeEquivalentTo(new[]
        {
                profile with { Id = ids[0], Key = profile.Key with { UserId = "user-1" } },
                profile with { Id = ids[2], Key = profile.Key with { UserId = "user-1" } }
        });
    }

    #endregion
}

