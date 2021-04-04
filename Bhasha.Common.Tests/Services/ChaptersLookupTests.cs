using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class ChaptersLookupTests
    {
        private IDatabase _database;
        private IAssembleChapters _chapters;
        private IChaptersLookup _lookup;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _chapters = A.Fake<IAssembleChapters>();
            _lookup = new ChaptersLookup(_database, _chapters);
        }

        [Test]
        public async Task GetChapters()
        {
            var profile = ProfileBuilder.Default.Build();
            var stats = ChapterStatsBuilder.Default.WithCompleted(false).Build();

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(A<Guid>._, A<Guid>._))
                .Returns(Task.FromResult(stats));

            var chapters = new[] { GenericChapterBuilder.Default.WithId(stats.ChapterId).Build() };

            A.CallTo(() => _database.QueryChaptersByLevel(profile.Level))
                .Returns(Task.FromResult<IEnumerable<GenericChapter>>(chapters));

            var expectedChapter = new Chapter(Guid.NewGuid(), 1, "x", "x", new Page[0], false, default);

            A.CallTo(() => _chapters.Assemble(chapters[0], profile))
                .Returns(Task.FromResult(expectedChapter));

            var result = await _lookup.GetChapters(profile);

            Assert.That(result, Is.EquivalentTo(new[] { expectedChapter }));
        }
    }
}
