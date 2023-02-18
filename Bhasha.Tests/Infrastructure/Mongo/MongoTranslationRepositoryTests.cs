using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Domain;
using Bhasha.Infrastructure.Mongo;
using Bhasha.Infrastructure.Mongo.Extensions;
using FluentAssertions;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace Bhasha.Tests.Infrastructure.Mongo;

public class MongoTranslationRepositoryTests : IDisposable
{
    private const string DbName = "TestDB";
    private readonly MongoTranslationRepository _repository;
    private readonly MongoDbRunner _runner;
    private readonly MongoClient _client;

    public MongoTranslationRepositoryTests()
    {
        _runner = MongoDbRunner.Start();
        _client = new(_runner.ConnectionString);
        _repository = new MongoTranslationRepository(_client, new MongoSettings
        {
            DatabaseName = DbName
        });
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _runner.Dispose();
    }

    #region AddOrUpdate

    [Theory(Skip = "fails with github actions"), AutoData]
    public async Task GivenRepository_WhenAddOrUpdateNewTranslationWithoutId_ThenAddTranslationWithIdToDatabase(Translation translation)
    {
        // act
        await _repository.AddOrReplace(translation);

        // verify
        var result = await _client
            .GetCollection<Translation>(DbName)
            .AsQueryable().SingleAsync();

        result.Id.Should().NotBe(Guid.Empty);
        result.Should().Be(translation with { Id = result.Id });
    }

    [Theory(Skip = "fails with github actions"), AutoData]
    public async Task GivenRepository_WhenAddOrUpdateExistingTranslation_ThenReplaceTranslationInDatabase(Translation translation)
    {
        // setup
        await _client
            .GetCollection<Translation>(DbName)
            .InsertOneAsync(translation with { Text = translation.Text + "[OLD]" });

        // act
        await _repository.AddOrReplace(translation);

        // verify
        var result = await _client
            .GetCollection<Translation>(DbName)
            .AsQueryable().SingleAsync();

        result.Should().Be(translation);
    }

    #endregion

    #region Find

    [Theory(Skip = "fails with github actions"), AutoData]
    public async Task GivenTranslationInDatabase_WhenFindByExpressionIdAndLanguage_ThenReturnTranslation(Translation translation)
    {
        // setup
        translation = translation with { Language = Language.Bengali };

        await _client
            .GetCollection<Translation>(DbName)
            .InsertOneAsync(translation);

        // act
        var result = await _repository.Find(translation.ExpressionId, translation.Language);

        // verify
        result.Should().Be(translation);
    }

    [Theory(Skip = "fails with github actions"), AutoData]
    public async Task GivenEmptyDatabase_WhenFindByExpressionIdAndLanguage_ThenReturnNull(Guid expressionId, Language language)
    {
        // act
        var result = await _repository.Find(expressionId, language);

        // verify
        result.Should().BeNull();
    }

    [Theory(Skip = "fails with github actions"), AutoData]
    public async Task GivenTranslationInDatabase_WhenFindByText_ThenReturnTranslation(Translation translation)
    {
        // setup
        translation = translation with { Language = Language.Bengali };

        await _client
            .GetCollection<Translation>(DbName)
            .InsertOneAsync(translation);

        // act
        var result = await _repository.Find(translation.Text);

        // verify
        result.Should().Be(translation);
    }

    [Theory(Skip = "fails with github actions"), AutoData]
    public async Task GivenEmptyDatabase_WhenFindByText_ThenReturnNull(string text)
    {
        // act
        var result = await _repository.Find(text);

        // verify
        result.Should().BeNull();
    }

    #endregion

    #region Get

    [Theory(Skip = "fails with github actions"), AutoData]
    public async Task GivenTranslationInDatabase_WhenGetById_ThenReturnTranslation(Translation translation)
    {
        // setup
        translation = translation with { Language = Language.Bengali };

        await _client
            .GetCollection<Translation>(DbName)
            .InsertOneAsync(translation);

        // act
        var result = await _repository.Get(translation.Id);

        // verify
        result.Should().Be(translation);
    }

    #endregion
}

