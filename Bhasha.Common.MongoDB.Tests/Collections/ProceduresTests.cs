using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Collections;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Collections
{
    [TestFixture]
    public class ProceduresTests
    {
        private IMongoDb _database;
        private Procedures _procedures;

        internal static IEnumerable Queries
        {
            get
            {
                yield return new TestCaseData(new ProcedureIdQuery(1, new ProcedureId("P1")));
                yield return new TestCaseData(new ProcedureSupportQuery(1, TokenType.Adjective));
            }
        }

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IMongoDb>();
            _procedures = new Procedures(_database);
        }

        [Test]
        public void Query_unsupported()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await _procedures.Query(new UnsupportedProcedureQuery(123)));
        }

        [TestCaseSource(nameof(Queries))]
        public async Task Query_existing_procedure(ProcedureQuery query)
        {
            var procedure = new ProcedureDto
            {
                ProcedureId = "P1",
                Description = "Test description of the procedure",
                Support = new string[0]
            };

            A.CallTo(() => _database.Find(
                Names.Collections.Procedures,
                A<Expression<Func<ProcedureDto, bool>>>._, 1)
            ).Returns(
                new ValueTask<IEnumerable<ProcedureDto>>(new[] { procedure }));

            var result = await _procedures.Query(query);

            var array = result.ToArray();

            Assert.That(array.Length == 1);
            Assert.That(array[0].Id.Id == procedure.ProcedureId);
            Assert.That(array[0].Description == procedure.Description);
            Assert.That(array[0].Support.Length == 0);
        }

        private class UnsupportedProcedureQuery : ProcedureQuery
        {
            public UnsupportedProcedureQuery(int maxItems) : base(maxItems)
            {
            }
        }
    }
}
