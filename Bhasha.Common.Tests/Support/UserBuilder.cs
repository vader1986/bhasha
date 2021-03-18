using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class UserBuilder
    {
        private Guid _id;
        private string _userName = Rnd.Create.NextString();
        private string _email = Rnd.Create.NextString();

        public UserBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public static UserBuilder Default => new();

        public User Build()
        {
            return new User(
                _id,
                _userName,
                _email);
        }
    }
}
