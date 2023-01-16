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

namespace Bhasha.Web.Tests.Mongo;

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

    [Theory, AutoData]
    public async Task GivenNewChapter_WhenUpserted_ThenChapterIdUpdated(Chapter chapter)
    {
        // setup
        chapter = chapter with { Id = Guid.Empty };

        // act
        var insertedChapter = await _repository.Upsert(chapter);

        // verify
        insertedChapter.Id.Should().NotBe(Guid.Empty);
    }
}