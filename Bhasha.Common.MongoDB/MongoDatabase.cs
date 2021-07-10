using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.MongoDB.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bhasha.Common.MongoDB
{
    public class MongoDatabase : IDatabase
    {
        private readonly IMongoClient _client;
        private readonly string _databaseName;

        public MongoDatabase(IMongoClient client, string databaseName)
        {
            _client = client;
            _databaseName = databaseName;
        }

        public async Task<IEnumerable<DbChapter>> QueryChapters(int level)
        {
            return await _client
               .Collection<DbChapter>(_databaseName)
               .AsQueryable()
               .Where(x => x.Level == level)
               .ToListAsync();
        }

        public async Task<IEnumerable<DbUserProfile>> QueryProfiles(string userId)
        {
            return await _client
               .Collection<DbUserProfile>(_databaseName)
               .AsQueryable()
               .Where(x => x.UserId == userId)
               .ToListAsync();
        }

        public async Task<DbStats?> QueryStats(Guid chapterId, Guid profileId)
        {
            return await _client
               .Collection<DbStats>(_databaseName)
               .AsQueryable()
               .FirstOrDefaultAsync(x => x.ChapterId == chapterId &&
                                         x.ProfileId == profileId);
        }

        public async Task<IEnumerable<DbStats>> QueryStats(Guid profileId)
        {
            return await _client
               .Collection<DbStats>(_databaseName)
               .AsQueryable()
               .Where(x => x.ProfileId == profileId)
               .ToListAsync();
        }
    }
}
