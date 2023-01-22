using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Infrastructure.Mongo;
using FluentAssertions;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace Bhasha.Tests.Mongo;

public class MongoChapterRepositoryTests : IDisposable
{
    private const string DbName = "TestDB";
    private readonly MongoDbRunner _runner;
    private readonly MongoChapterRepository _repository;
    private readonly MongoClient _client;

    public MongoChapterRepositoryTests()
    {
        _runner = MongoDbRunner.Start();
        _client = new(_runner.ConnectionString);
        _repository = new MongoChapterRepository(_client, new MongoSettings {
            DatabaseName = DbName
        });
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _runner.Dispose();
    }

    #region AddOrReplace

    [Theory, AutoData]
    public async Task GivenChapterWithEmtpyId_WhenAddOrReplace_ThenReturnChapterWithNewGuid(Chapter chapter)
    {
        // setup
        chapter = chapter with { Id = Guid.Empty };

        // act
        var insertedChapter = await _repository.AddOrReplace(chapter);

        // verify
        insertedChapter.Id.Should().NotBe(Guid.Empty);
    }

    [Theory, AutoData]
    public async Task GivenChapterWithId_WhenAddOrReplace_ThenInsertChapter(Chapter chapter)
    {
        // act
        await _repository.AddOrReplace(chapter);

        // verify
        var insertedChapter = await _client
            .GetDatabase(DbName)
            .GetCollection<Chapter>("Chapter")
            .AsQueryable()
            .SingleAsync();

        insertedChapter.Should().Be(chapter);
    }

    [Theory, AutoData]
    public async Task GivenChapterWithIdOfExistingChapter_WhenAddOrReplace_ThenReplaceExistingChapter(Chapter existingChapter, Chapter chapter)
    {
        // setup
        await _client
            .GetDatabase(DbName)
            .GetCollection<Chapter>("Chapter")
            .InsertOneAsync(existingChapter with { Id = chapter.Id });

        // act
        await _repository.AddOrReplace(chapter);

        // verify
        var insertedChapter = await _client
            .GetDatabase(DbName)
            .GetCollection<Chapter>("Chapter")
            .AsQueryable()
            .SingleAsync();

        insertedChapter.Should().Be(chapter);
    }

    #endregion

    #region FindById

    [Theory, AutoData]
    public async Task GivenNoChapterInDatabase_WhenFindById_ThenReturnNull(Guid chapterId)
    {
        // act
        var result = await _repository.FindById(chapterId);

        // verify
        result.Should().BeNull();
    }

    [Theory, AutoData]
    public async Task GivenChapterInDatabase_WhenFindById_ThenReturnChapter(Chapter chapter)
    {
        // setup
        await _client
            .GetDatabase(DbName)
            .GetCollection<Chapter>("Chapter")
            .InsertOneAsync(chapter);

        // act
        var result = await _repository.FindById(chapter.Id);

        // verify
        result.Should().Be(chapter);
    }

    #endregion

    #region FindByLevel

    [Theory, AutoData]
    public async Task GivenNoChapterInDatabase_WhenFindByLevel_ThenReturnEmpty(int level)
    {
        // act
        var result = await _repository.FindByLevel(level).ToArrayAsync();

        // verify
        result.Should().BeEmpty();
    }

    [Theory, AutoData]
    public async Task GivenChaptersInDatabase_WhenFindByLevel_ThenReturnChaptersWithLevel(Chapter chapter)
    {
        // setup
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        await _client
            .GetDatabase(DbName)
            .GetCollection<Chapter>("Chapter")
            .InsertManyAsync(new[]
            {
                chapter with { Id = ids[0], RequiredLevel = 1 },
                chapter with { Id = ids[1], RequiredLevel = 2 },
                chapter with { Id = ids[2], RequiredLevel = 3 }
            });

        // act
        var result = await _repository.FindByLevel(2).ToArrayAsync();

        // verify
        result.Should().BeEquivalentTo(new[]
        {
            chapter with { Id = ids[1], RequiredLevel = 2 }
        });
    }

    #endregion
}