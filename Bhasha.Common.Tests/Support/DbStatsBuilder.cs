using System;
using System.Linq;
using Bhasha.Common.Database;

namespace Bhasha.Common.Tests.Support
{
    public class DbStatsBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _profileId = Guid.NewGuid();
        private Guid _chapterId = Guid.NewGuid();
        private bool _completed = true;
        private byte[] _tips = Enumerable.Range(0, 5).Select(x => (byte)x).ToArray();
        private byte[] _submits = Enumerable.Range(0, 5).Select(x => (byte)x).ToArray();
        private byte[] _failures = Enumerable.Range(0, 5).Select(x => (byte)x).ToArray();

        public static DbStatsBuilder Default => new();

        public DbStatsBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public DbStatsBuilder WithProfileId(Guid profileId)
        {
            _profileId = profileId;
            return this;
        }

        public DbStatsBuilder WithChapterId(Guid chapterId)
        {
            _chapterId = chapterId;
            return this;
        }

        public DbStatsBuilder WithCompleted(bool completed)
        {
            _completed = completed;
            return this;
        }

        public DbStatsBuilder WithTips(byte[] tips)
        {
            _tips = tips;
            return this;
        }

        public DbStatsBuilder WithSubmits(byte[] submits)
        {
            _submits = submits;
            return this;
        }

        public DbStatsBuilder WithFailures(byte[] failures)
        {
            _failures = failures;
            return this;
        }

        public DbStats Build()
        {
            return new DbStats {
                Id = _id,
                ChapterId = _chapterId,
                ProfileId = _profileId,
                Completed = _completed,
                Tips = _tips,
                Submits = _submits,
                Failures = _failures
            };
        }
    }
}
