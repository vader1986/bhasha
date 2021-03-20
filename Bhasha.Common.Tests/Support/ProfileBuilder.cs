using System;
using System.Linq;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class ProfileBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _userId = Rnd.Create.NextString();
        private Language _from = Language.English;
        private Language _to = Language.Bengali;
        private int _level = Rnd.Create.Next();
        private int _completedChapters = Rnd.Create.Next();

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

        public ProfileBuilder WithFrom(Language from)
        {
            _from = from;
            return this;
        }

        public ProfileBuilder WithTo(Language to)
        {
            _to = to;
            return this;
        }

        public static ProfileBuilder Default = new();

        public Profile Build()
        {
            return new Profile(
                _id,
                _userId,
                _from,
                _to,
                _level,
                _completedChapters);
        }
    }
}
