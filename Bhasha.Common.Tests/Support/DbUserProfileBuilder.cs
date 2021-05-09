using System;
using Bhasha.Common.Database;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class DbUserProfileBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _userId = Rnd.Create.NextString();
        private DbProfile _languages = DbProfileBuilder.Default.Build();
        private int _level = Rnd.Create.Next();
        private int _completedChapters = Rnd.Create.Next();

        public static DbUserProfileBuilder Default => new();

        public DbUserProfileBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public DbUserProfileBuilder WithUserId(string userId)
        {
            _userId = userId;
            return this;
        }

        public DbUserProfileBuilder WithLanguages(DbProfile languages)
        {
            _languages = languages;
            return this;
        }

        public DbUserProfileBuilder WithLevel(int level)
        {
            _level = level;
            return this;
        }

        public DbUserProfileBuilder WithCompletedChapters(int completedChapters)
        {
            _completedChapters = completedChapters;
            return this;
        }

        public DbUserProfile Build()
        {
            return new DbUserProfile {
                Id = _id,
                UserId = _userId,
                Level = _level,
                CompletedChapters = _completedChapters,
                Languages = _languages
            };
        }
    }
}
