using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;

namespace Bhasha.Common.MongoDB.Collections
{
    public class UserProgressCollection : IQueryable<UserProgress, UserProgressQuery>
    {
        private readonly IMongoDb _database;

        public UserProgressCollection(IMongoDb database)
        {
            _database = database;
        }

        public ValueTask<IEnumerable<UserProgress>> Query(UserProgressQuery query)
        {
            return query switch
            {
                UserProgressQueryById byId => ExecuteQuery(byId),
                _ => OnDefault(query)
            };
        }

        private static ValueTask<IEnumerable<UserProgress>> OnDefault(UserProgressQuery query)
        {
            throw new ArgumentOutOfRangeException(
                nameof(query),
                $"Query type {query.GetType().FullName} not supported");
        }

        private async ValueTask<IEnumerable<UserProgress>> ExecuteQuery(UserProgressQueryById query)
        {
            var result = await _database.Find<UserProgressDto>(
                Names.Collections.Users,
                x => x.UserId == query.UserId.Id, query.MaxItems);

            return result.Select(Converter.Convert);
        }
    }
}
