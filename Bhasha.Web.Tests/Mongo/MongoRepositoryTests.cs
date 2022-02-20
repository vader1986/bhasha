using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Bhasha.Web.Mongo;
using Bhasha.Web.Services;
using Mongo2Go;
using MongoDB.Driver;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Mongo;

/*
 * ToDo:
 * - add missing unit tests for new Find function
 */

public class MongoRepositoryTests
{
    private const string DbName = "TestDB";
    private MongoDbRunner _runner = default!;
    private MongoRepository<Item> _repository = default!;
    private MongoClient _client = default!;

    [SetUp]
    public void Before()
    {
        _runner = MongoDbRunner.Start();
        _client = new MongoClient(_runner.ConnectionString);
        _repository = new MongoRepository<Item>(_client, new MongoSettings {
            DatabaseName = DbName
        });
    }

    [TearDown]
    public void After()
    {
        _runner.Dispose();
    }

    private IMongoCollection<Item> GetCollection()
    {
        return _client
            .GetDatabase(DbName)
            .GetCollection<Item>("Item");
    }

    [Test]
    public async Task GivenAnItem_WhenAdded_ThenReturnItemWithNewId()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");

        // act
        var result = await _repository.Add(item);

        // verify
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Name, Is.EqualTo("car"));
    }

    [Test]
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
        Assert.AreEqual(itemFound.Name, "car");
    }

    [Test]
    public async Task GivenItem_WhenGet_ThenReturnItem()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");
        await GetCollection().InsertOneAsync(item);

        // act
        var result = await _repository.Get(item.Id);

        // verify
        Assert.AreEqual(item, result);
    }

    [Test]
    public async Task GivenNoItem_WhenGet_ThenReturnNull()
    {
        // act
        var result = await _repository.Get(Guid.NewGuid());

        // verify
        Assert.IsNull(result);
    }

    [Test, AutoData]
    public async Task GivenItems_WhenGetMany_ThenReturnItems(Item[] items)
    {
        // prepare
        await GetCollection().InsertManyAsync(items);

        // act
        var result = await _repository.GetMany(items.Select(x => x.Id).ToArray());

        // assert
        Assert.That(result, Is.EquivalentTo(items));
    }

    [Test]
    public async Task GivenMatchingItemInDB_WhenFindCalled_ThenItemReturned()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");
        await GetCollection().InsertOneAsync(item);

        // act
        var result = await _repository.Find(x => x.Name == "car");

        // verify
        Assert.AreEqual(result, new[] { item });
    }

    [Test]
    public async Task GivenNoMatchingItemInDB_WhenFindCalled_ThenReturnEmptyArray()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");
        await GetCollection().InsertOneAsync(item);

        // act
        var result = await _repository.Find(x => x.Name == "airplane");

        // verify
        Assert.IsEmpty(result);
    }

    [Test]
    public async Task GivenItemInDB_WhenRemoved_ThenReturnTrue()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");
        await GetCollection().InsertOneAsync(item);

        // act
        var itemHasBeenRemoved = await _repository.Remove(item.Id);

        // verify
        Assert.That(itemHasBeenRemoved);
    }

    [Test]
    public async Task GivenNoItemInDB_WhenRemoved_ThenReturnFalse()
    {
        // act
        var itemHasBeenRemoved = await _repository.Remove(Guid.NewGuid());

        // verify
        Assert.That(itemHasBeenRemoved, Is.False);
    }

    [Test]
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
        Assert.That(await result.AnyAsync(), Is.False);
    }

    [Test]
    public async Task GivenNonMatchingItemInDB_WhenRemoveCalled_ThenItemIsNotDeletedFromDB()
    {
        // prepare
        var collection = GetCollection();
        var item = new Item(Guid.Empty, "car");
        await collection.InsertOneAsync(item);

        // act
        var hasBeenRemoved = await _repository.Remove(Guid.NewGuid());

        // verify
        Assert.That(hasBeenRemoved, Is.False);
        var result = await collection.FindAsync(x => x.Id == item.Id);
        Assert.That(await result.AnyAsync(), Is.True);
    }

    [Test, AutoData]
    public async Task GivenItemInDB_WhenUpdated_ThenReturnTrue(Item item)
    {
        // prepare
        var collection = GetCollection();
        await collection.InsertOneAsync(item);

        // act
        var hasBeenUpdated = await _repository.Update(item.Id, item with { Name = "truck" });

        // verify
        Assert.That(hasBeenUpdated);
    }

    [Test, AutoData]
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

        Assert.That(updatedItem.Name, Is.EqualTo("truck"));
    }

    [Test]
    public async Task GivenNoItemInDB_WhenUpdateCalled_ThenReturnFalse()
    {
        // prepare
        var item = new Item(Guid.Empty, "car");

        // act
        var hasBeenUpdated = await _repository.Update(item.Id, item with { Name = "truck" });

        // verify
        Assert.That(hasBeenUpdated, Is.False);
    }
}

public record Item(Guid Id, string Name);