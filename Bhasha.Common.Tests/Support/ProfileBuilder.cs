using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class ProfileBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _userId = Rnd.Create.NextString();
        private Language _native = Language.English;
        private Language _target = Language.Bengali;
        private int _level = Rnd.Create.Next(1, 10);
        private int _completedChapters = Rnd.Create.Next(1, 10);

        public static ProfileBuilder Default => new();

        public ProfileBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ProfileBuilder WithUserId(string userId)
        {
            _userId = userId;
            return this;
        }

        public ProfileBuilder WithNative(Language native)
        {
            _native = native;
            return this;
        }

        public ProfileBuilder WithTarget(Language target)
        {
            _target = target;
            return this;
        }

        public ProfileBuilder WithLevel(int level)
        {
            _level = level;
            return this;
        }

        public ProfileBuilder WithCompletedChapters(int completedChapters)
        {
            _completedChapters = completedChapters;
            return this;
        }

        public Profile Build()
        {
            return new Profile(_id, _userId, _native, _target, _level, _completedChapters);
        }
    }
}
