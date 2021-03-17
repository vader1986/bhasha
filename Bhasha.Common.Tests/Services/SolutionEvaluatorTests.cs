using Bhasha.Common.Services;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class SolutionEvaluatorTests
    {
        private SolutionEvaluator _evaluator;

        [SetUp]
        public void Before()
        {
            _evaluator = new SolutionEvaluator();
        }

        [Test]
        public void Evaluate_solution_equals_expected()
        {
            var evaluation = _evaluator.Evaluate("test", "test");

            Assert.That(evaluation.Result == Result.Correct);
        }

        [Test]
        public void Evaluate_solution_not_equals_expected()
        {
            var evaluation = _evaluator.Evaluate("test", "123");

            Assert.That(evaluation.Result == Result.Wrong);
        }
    }
}
