using Bhasha.Common.Extensions;
using Bhasha.Common.Tests.Support;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class ChapterStatsExtensionsTests
    {
        [Test]
        public void WithCompleted()
        {
            var stats = ChapterStatsBuilder
                .Default
                .WithCompleted(false)
                .Build();

            var result = stats.WithCompleted();

            Assert.That(result.Completed, Is.True);
        }
    }
}
