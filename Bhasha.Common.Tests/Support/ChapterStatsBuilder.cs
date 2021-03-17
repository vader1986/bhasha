using System;
using System.Linq;

namespace Bhasha.Common.Tests.Support
{
    public class ChapterStatsBuilder
    {
        private Guid _profileId = Guid.NewGuid();
        private Guid _chapterId = Guid.NewGuid();
        private bool _completed = true;
        private byte[] _tips = Enumerable.Range(0, 5).Select(x => (byte)x).ToArray();
        private byte[] _submits = Enumerable.Range(0, 5).Select(x => (byte)x).ToArray();
        private byte[] _failures = Enumerable.Range(0, 5).Select(x => (byte)x).ToArray();

        public static ChapterStatsBuilder Default => new ChapterStatsBuilder();

        public ChapterStatsBuilder WithCompleted(bool completed)
        {
            _completed = completed;
            return this;
        }

        public ChapterStatsBuilder WithTips(byte[] tips)
        {
            _tips = tips;
            return this;
        }

        public ChapterStatsBuilder WithSubmits(byte[] submits)
        {
            _submits = submits;
            return this;
        }

        public ChapterStatsBuilder WithFailures(byte[] failures)
        {
            _failures = failures;
            return this;
        }

        public ChapterStats Build()
        {
            return new ChapterStats(
                _profileId,
                _chapterId,
                _completed,
                _tips,
                _submits,
                _failures);
        }
    }
}
