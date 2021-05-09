using System;
using System.Linq;

namespace Bhasha.Common.Tests.Support
{
    public class StatsBuilder
    {
        private Guid _profileId = Guid.NewGuid();
        private Guid _chapterId = Guid.NewGuid();
        private bool _completed;
        private byte[] _tips = Enumerable.Repeat((byte)0, 10).ToArray();
        private byte[] _submits = Enumerable.Repeat((byte)0, 10).ToArray();
        private byte[] _failures = Enumerable.Repeat((byte)0, 10).ToArray();

        public static StatsBuilder Default => new();

        public StatsBuilder WithProfileId(Guid profileId)
        {
            _profileId = profileId;
            return this;
        }

        public StatsBuilder WithChapterId(Guid chapterId)
        {
            _chapterId = chapterId;
            return this;
        }

        public StatsBuilder WithCompleted(bool completed)
        {
            _completed = completed;
            return this;
        }

        public StatsBuilder WithTips(byte[] tips)
        {
            _tips = tips;
            return this;
        }

        public StatsBuilder WithSubmits(byte[] submits)
        {
            _submits = submits;
            return this;
        }

        public StatsBuilder WithFailures(byte[] failures)
        {
            _failures = failures;
            return this;
        }

        public Stats Build()
        {
            return new Stats(_profileId, _chapterId, _completed, _tips, _submits, _failures);
        }
    }
}
