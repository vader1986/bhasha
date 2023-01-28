using System;
using System.Collections.Generic;
using System.Linq;
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

public class MongoExpressionRepositoryTests : IDisposable
{
    private const string DbName = "TestDB";
    private readonly MongoExpressionRepository _repository;
    private readonly MongoDbRunner _runner;
    private readonly MongoClient _client;

    public MongoExpressionRepositoryTests()
    {
        _runner = MongoDbRunner.Start();
        _client = new(_runner.ConnectionString);
        _repository = new MongoExpressionRepository(_client, new MongoSettings
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
    public async Task GivenEmptyDatabase_WhenAddExpressionWithEmptyId_ThenReturnExpressionWithNewId(Expression expression)
    {
        // act
        var result = await _repository.Add(expression with { Id = Guid.Empty });

        // verify
        result.Id.Should().NotBe(Guid.Empty);
    }

    [Theory, AutoData]
    public async Task GivenEmptyDatabase_WhenAddExpression_ThenReturnExpression(Expression expression)
    {
        // act
        var result = await _repository.Add(expression);

        // verify
        result.Should().Be(expression);
    }

    [Theory, AutoData]
    public async Task GivenEmptyDatabase_WhenAddExpression_ThenAddExpressionToDatabase(Expression expression)
    {
        // act
        await _repository.Add(expression);

        // verify
        var insertedExpression = await _client
            .GetCollection<Expression>(DbName)
            .AsQueryable().SingleAsync();

        insertedExpression.Should().Be(expression);
    }

    #endregion

    #region Find

    [Theory, AutoData]
    public async Task GivenDatabase_WhenFindByLevelAndSamples_ThenReturnExpressionsForLevel(Expression expression)
    {
        // verify
        var expressions = new List<Expression>
        {
            expression with { Id = Guid.NewGuid(), Level = 1 },
            expression with { Id = Guid.NewGuid(), Level = 1 },
            expression with { Id = Guid.NewGuid(), Level = 2 },
            expression with { Id = Guid.NewGuid(), Level = 2 },
            expression with { Id = Guid.NewGuid(), Level = 1 }
        };

        await _client
            .GetCollection<Expression>(DbName)
            .InsertManyAsync(expressions);

        // act
        var results = await _repository.Find(1, 3).ToListAsync();

        // verify
        results.Should().BeEquivalentTo(new List<Expression>
        {
            expressions[0], expressions[1], expressions[4]
        });
    }

    [Theory, AutoData]
    public async Task GivenDatabaseWithLessElementsThanSamples_WhenFindByLevelAndSamples_ThenReturnAllAvailableSamplesForLevel(Expression expression)
    {
        // verify
        var expressions = new List<Expression>
        {
            expression with { Id = Guid.NewGuid(), Level = 1 },
            expression with { Id = Guid.NewGuid(), Level = 1 },
            expression with { Id = Guid.NewGuid(), Level = 2 },
            expression with { Id = Guid.NewGuid(), Level = 2 },
            expression with { Id = Guid.NewGuid(), Level = 1 }
        };

        await _client
            .GetCollection<Expression>(DbName)
            .InsertManyAsync(expressions);

        // act
        var results = await _repository.Find(1, 5).ToListAsync();

        // verify
        results.Should().BeEquivalentTo(new List<Expression>
        {
            expressions[0], expressions[1], expressions[4]
        });
    }

    #endregion

    #region Get

    [Theory, AutoData]
    public async Task GivenDatabase_WhenGetById_ThenReturnElementWithId(Expression expression)
    {
        // verify
        var expressions = new List<Expression>
        {
            expression with { Id = Guid.NewGuid() },
            expression with { Id = Guid.NewGuid() },
            expression with { Id = Guid.NewGuid() },
            expression with { Id = Guid.NewGuid() },
            expression with { Id = Guid.NewGuid() }
        };

        await _client
            .GetCollection<Expression>(DbName)
            .InsertManyAsync(expressions);

        // act
        var result = await _repository.Get(expressions[3].Id);

        // verify
        result.Should().Be(expressions[3]);
    }

    #endregion
}