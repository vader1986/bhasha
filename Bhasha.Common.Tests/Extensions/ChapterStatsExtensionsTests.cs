using Bhasha.Common.Extensions;
using Bhasha.Common.Tests.Support;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class ChapterStatsExtensionsTests
    {
        [Test]
        public void WithCompleted_and_submits_equal_to_failures()
        {
            var stats = ChapterStatsBuilder
                .Default
                .WithSubmits(new byte[] { 1, 2, 3, 4 })
                .WithFailures(new byte[] { 0, 1, 3, 3 })
                .WithCompleted(true)
                .Build();

            var result = stats.WithCompleted();

            Assert.That(result.Completed, Is.False);
        }

        [Test]
        public void WithCompleted_and_all_submits_higher_than_failures()
        {
            var stats = ChapterStatsBuilder
                .Default
                .WithSubmits(new byte[] { 1, 2, 3, 4 })
                .WithFailures(new byte[] { 0, 1, 2, 3 })
                .WithCompleted(false)
                .Build();

            var result = stats.WithCompleted();

            Assert.That(result.Completed, Is.True);
        }

        [Test]
        public void WithCompleted_and_submits_and_failures_are_at_maximum()
        {
            var stats = ChapterStatsBuilder
                .Default
                .WithSubmits(new byte[] { 1, 2, byte.MaxValue, byte.MaxValue })
                .WithFailures(new byte[] { 0, 1, byte.MaxValue, byte.MaxValue })
                .WithCompleted(false)
                .Build();

            var result = stats.WithCompleted();

            Assert.That(result.Completed, Is.True);
        }
    }
}
