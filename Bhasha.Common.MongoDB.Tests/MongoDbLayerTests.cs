using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Tests.Support;
using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using System.Text;

namespace Bhasha.Common.MongoDB.Tests
{
    [TestFixture]
    public class MongoDbLayerTests
    {
        private MongoDbRunner _runner;
        private MongoDb _db;
        private MongoDbLayer _layer;

        [SetUp]
        public void Before()
        {
            _runner = MongoDbRunner.Start();
            _db = MongoDb.Create(_runner.ConnectionString);
            _layer = new MongoDbLayer(_db, new Converter());
        }

        [TearDown]
        public void After()
        {
            _runner.Dispose();
        }
    }
}
