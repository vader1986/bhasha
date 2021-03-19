using Bhasha.Common.Services;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class ResultCheckerTests
    {
        private ResultChecker _checker;

        [SetUp]
        public void Before()
        {
            _checker = new ResultChecker();
        }

        [Test]
        public void Evaluate_solution_equals_expected()
        {
            var result = _checker.Evaluate("test", "test");

            Assert.That(result == Result.Correct);
        }

        [Test]
        public void Evaluate_solution_not_equals_expected()
        {
            var result = _checker.Evaluate("test", "123");

            Assert.That(result == Result.Wrong);
        }
    }
}
