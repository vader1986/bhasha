using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Aggregation;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Extensions;
using Bhasha.Common.Queries;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Aggregation
{
    [TestFixture]
    public class ProcedureLoaderTests
    {
        private IQueryable<Procedure, ProcedureQuery> _procedures;
        private ProcedureLoader _loader;

        [SetUp]
        public void Before()
        {
            _procedures = A.Fake<IQueryable<Procedure, ProcedureQuery>>();
            _loader = new ProcedureLoader(_procedures);
        }

        [Test]
        public void NextProcedures_missing_procedure_for_token_type([Values]TokenType tokenType)
        {
            var reference = TokenBuilder
                .Default
                .WithTokenType(tokenType)
                .Build();

            var translations = TranslationBuilder
                .Default
                .WithReference(reference)
                .Build()
                .ToEnumeration()
                .ToArray();

            A.CallTo(() => _procedures.Query(A<ProcedureQuery>._)).Returns(
                new ValueTask<IEnumerable<Procedure>>(new Procedure[0]));

            Assert.ThrowsAsync<NoProcedureFoundException>(async () => await _loader.NextProcedures(translations));
        }

        [Test]
        public async Task NextProcedures_happy_path([Values] TokenType tokenType)
        {
            var reference = TokenBuilder
                .Default
                .WithTokenType(tokenType)
                .Build();

            var translations = TranslationBuilder
                .Default
                .WithReference(reference)
                .Build()
                .ToEnumeration()
                .ToArray();

            var procedures = new[] { new Procedure(new ProcedureId("p1"), "desc", null, null, new TokenType[0]) };

            A.CallTo(() => _procedures.Query(A<ProcedureQuery>._)).Returns(
                new ValueTask<IEnumerable<Procedure>>(procedures));

            var result = await _loader.NextProcedures(translations);

            Assert.That(result, Is.EquivalentTo(procedures));
        }
    }
}
