using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bhasha.Web.Mongo;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace Bhasha.Web.Tests.Mongo;

public class MongoRepositoryTests : IDisposable
{
    private const string DbName = "TestDB";
    private readonly MongoDbRunner _runner;
    private readonly MongoRepository<Item> _repository;
    private readonly MongoClient _client;

    public MongoRepositoryTests()
    {
        _runner = MongoDbRunner.Start();
        _client = new(_runner.ConnectionString);
        _repository = new MongoRepository<Item>(_client, new MongoSettings {
            DatabaseName = DbName
        });
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _runner.Dispose();
    }

    private IMongoCollection<Item> GetCollection()
    {
        return _client
            .GetDatabase(DbName)
            .GetCollection<Item>("Item");
    }

    [Fact]
    public async Task GivenAnItem_WhenAdded_ThenReturnItemWithNewId()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");

        // act
        var result = await _repository.Add(item);

        // verify
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("car", result.Name);
    }

    [Fact]
    public async Task GivenAnItem_WhenAdded_ThenItemAddedToMongoDb()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");

        // act
        var result = await _repository.Add(item);

        // verify
        var itemsFound = await GetCollection().FindAsync(x => x.Id == result.Id);
        var itemFound = await itemsFound.FirstOrDefaultAsync();

        Assert.NotNull(itemFound);
        Assert.Equal("car", itemFound.Name);
    }

    [Fact]
    public async Task GivenItem_WhenGet_ThenReturnItem()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");
        await GetCollection().InsertOneAsync(item);

        // act
        var result = await _repository.Get(item.Id);

        // verify
        Assert.Equal(item, result);
    }

    [Fact]
    public async Task GivenNoItem_WhenGet_ThenReturnNull()
    {
        // act
        var result = await _repository.Get(Guid.NewGuid());

        // verify
        Assert.Null(result);
    }

    [Theory, AutoData]
    public async Task GivenItems_WhenGetMany_ThenReturnItems(Item[] items)
    {
        // prepare
        await GetCollection().InsertManyAsync(items);

        // act
        var result = await _repository.GetMany(items.Select(x => x.Id).ToArray());

        // assert
        Assert.Equal(items, result);
    }

    [Fact]
    public async Task GivenMatchingItemInDB_WhenFindCalled_ThenItemReturned()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");
        await GetCollection().InsertOneAsync(item);

        // act
        var result = await _repository.Find(x => x.Name == "car");

        // verify
        Assert.Equal(result, new[] { item });
    }

    [Fact]
    public async Task GivenNoMatchingItemInDB_WhenFindCalled_ThenReturnEmptyArray()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");
        await GetCollection().InsertOneAsync(item);

        // act
        var result = await _repository.Find(x => x.Name == "airplane");

        // verify
        Assert.Empty(result);
    }

    [Fact]
    public async Task GivenItemInDB_WhenRemoved_ThenReturnTrue()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");
        await GetCollection().InsertOneAsync(item);

        // act
        var itemHasBeenRemoved = await _repository.Remove(item.Id);

        // verify
        Assert.True(itemHasBeenRemoved);
    }

    [Fact]
    public async Task GivenNoItemInDB_WhenRemoved_ThenReturnFalse()
    {
        // act
        var itemHasBeenRemoved = await _repository.Remove(Guid.NewGuid());

        // verify
        Assert.False(itemHasBeenRemoved);
    }

    [Fact]
    public async Task GivenItemInDB_WhenRemoved_ThenItemHasBeenDeletedFromDB()
    {
        // prepare
        var collection = GetCollection();
        var item = new Item(Guid.Empty, "car");
        await collection.InsertOneAsync(item);

        // act
        await _repository.Remove(item.Id);

        // verify
        var result = await collection.FindAsync(x => x.Id == item.Id);
        Assert.False(await result.AnyAsync());
    }

    [Fact]
    public async Task GivenNonMatchingItemInDB_WhenRemoveCalled_ThenItemIsNotDeletedFromDB()
    {
        // prepare
        var collection = GetCollection();
        var item = new Item(Guid.Empty, "car");
        await collection.InsertOneAsync(item);

        // act
        var hasBeenRemoved = await _repository.Remove(Guid.NewGuid());

        // verify
        Assert.False(hasBeenRemoved);
        var result = await collection.FindAsync(x => x.Id == item.Id);
        Assert.True(await result.AnyAsync());
    }

    [Theory, AutoData]
    public async Task GivenItemInDB_WhenUpdated_ThenReturnTrue(Item item)
    {
        // prepare
        var collection = GetCollection();
        await collection.InsertOneAsync(item);

        // act
        var hasBeenUpdated = await _repository.Update(item.Id, item with { Name = "truck" });

        // verify
        Assert.True(hasBeenUpdated);
    }

    [Theory, AutoData]
    public async Task GivenItemInDB_WhenUpdated_ThenItemUpdatedInDB(Item item)
    {
        // prepare
        var collection = GetCollection();
        await collection.InsertOneAsync(item);

        // act
        var hasBeenUpdated = await _repository.Update(item.Id, item with { Name = "truck" });

        // verify
        var result = await collection.FindAsync(x => x.Id == item.Id);
        var updatedItem = await result.SingleAsync();

        Assert.Equal("truck", updatedItem.Name);
    }

    [Fact]
    public async Task GivenNoItemInDB_WhenUpdateCalled_ThenReturnFalse()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");

        // act
        var hasBeenUpdated = await _repository.Update(item.Id, item with { Name = "truck" });

        // verify
        Assert.False(hasBeenUpdated);
    }

    [Theory, AutoData]
    public async Task GivenItemsInDB_WhenFind5Samples_ThenReturn5Samples(Item[] items)
    {
        // prepare
        await GetCollection().InsertManyAsync(items);

        // act
        var results = await _repository.Find(_ => true, items.Length - 1);

        // verify
        Assert.Equal(items.Length - 1, results.Length);
    }
}

public record Item(Guid Id, string Name);