using System;
using System.Linq;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class ProfileBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();
        private Language _from = Rnd.Create.Choose(Language.Supported.Values.ToArray());
        private Language _to = Rnd.Create.Choose(Language.Supported.Values.ToArray());
        private int _level = Rnd.Create.Next();

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
                _level);
        }
    }
}
