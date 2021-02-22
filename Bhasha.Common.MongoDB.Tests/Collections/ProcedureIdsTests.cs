using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Collections;
using Bhasha.Common.MongoDB.Dto;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Collections
{
    [TestFixture]
    public class ProcedureIdsTests
    {
        private IMongoDb _database;
        private ProcedureIds _procedureIds;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IMongoDb>();
            _procedureIds = new ProcedureIds(_database);
        }

        [Test]
        public async Task List_procedure_ids_from_database()
        {
            A.CallTo(() => _database.List(
                Names.Collections.Procedures,
                A<Expression<Func<ProcedureDto, string>>>._)
            ).Returns(
                new ValueTask<IEnumerable<string>>(new[] { "P-123", "P-212", "P-222" }));

            var result = await _procedureIds.List();

            Assert.That(result, Is.EquivalentTo(new[] {
                new ProcedureId("P-123"), new ProcedureId("P-212"), new ProcedureId("P-222")
            }));
        }
    }
}
