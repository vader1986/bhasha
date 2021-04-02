using System;
using System.Linq;

namespace Bhasha.Common.Tests.Support
{
    public class ChapterStatsBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _profileId = Guid.NewGuid();
        private Guid _chapterId = Guid.NewGuid();
        private bool _completed = true;
        private int _tips = Rnd.Create.Next(0, 10);
        private byte[] _submits = Enumerable.Range(0, 5).Select(x => (byte)x).ToArray();
        private byte[] _failures = Enumerable.Range(0, 5).Select(x => (byte)x).ToArray();

        public static ChapterStatsBuilder Default => new();

        public ChapterStatsBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ChapterStatsBuilder WithProfileId(Guid profileId)
        {
            _profileId = profileId;
            return this;
        }

        public ChapterStatsBuilder WithChapterId(Guid chapterId)
        {
            _chapterId = chapterId;
            return this;
        }

        public ChapterStatsBuilder WithCompleted(bool completed)
        {
            _completed = completed;
            return this;
        }

        public ChapterStatsBuilder WithTips(int tips)
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
                _id,
                _profileId,
                _chapterId,
                _completed,
                _tips,
                _submits,
                _failures);
        }
    }
}
