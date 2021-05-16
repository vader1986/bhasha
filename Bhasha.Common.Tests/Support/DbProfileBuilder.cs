using Bhasha.Common.Database;

namespace Bhasha.Common.Tests.Support
{
    public class DbProfileBuilder
    {
        private Language _native = Language.English;
        private Language _target = Language.Bengali;

        public static DbProfileBuilder Default => new();

        public DbProfileBuilder WithNative(Language native)
        {
            _native = native;
            return this;
        }

        public DbProfileBuilder WithTarget(Language target)
        {
            _target = target;
            return this;
        }

        public DbProfile Build()
        {
            return new DbProfile {
                Native = _native,
                Target = _target
            };
        }
    }
}
