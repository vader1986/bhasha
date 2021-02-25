using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Collections;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Tests.Support;
using Bhasha.Common.Queries;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.MongoDB.Tests.Collections
{
    [TestFixture]
    public class UserProgressCollectionTests
    {
        private IMongoDb _database;
        private UserProgressCollection _users;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IMongoDb>();
            _users = new UserProgressCollection(_database);
        }

        [Test]
        public void Query_unsupported()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
                await _users.Query(new UnsupportedQuery()));
        }

        [Test]
        public async Task Query_existing_user_progress()
        {
            var progress = UserProgressDtoBuilder.Create();
            var query = new UserProgressQueryById(1, new EntityId(progress.UserId));

            A.CallTo(() => _database.Find(Names.Collections.Users,
                A<Expression<Func<UserProgressDto, bool>>>._, 1)
            ).Returns(
                new ValueTask<IEnumerable<UserProgressDto>>(progress.ToEnumeration()));

            var userResults = await _users.Query(query);
            var result = userResults.Single();

            Assert.That(result.UserId, Is.EqualTo(new EntityId(progress.UserId)));
        }

        private class UnsupportedQuery : UserProgressQuery
        {
            public UnsupportedQuery() : base(1)
            {
            }
        }
    }
}
