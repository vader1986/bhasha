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
        private readonly IMongoClient _db;

        public MongoDatabase(IMongoClient db)
        {
            _db = db;
        }

        public async Task<IEnumerable<DbChapter>> QueryChapters(int level)
        {
            return await _db
               .Collection<DbChapter>()
               .AsQueryable()
               .Where(x => x.Level == level)
               .ToListAsync();
        }

        public async Task<IEnumerable<DbUserProfile>> QueryProfiles(string userId)
        {
            return await _db
               .Collection<DbUserProfile>()
               .AsQueryable()
               .Where(x => x.UserId == userId)
               .ToListAsync();
        }

        public async Task<DbStats?> QueryStats(Guid chapterId, Guid profileId)
        {
            return await _db
               .Collection<DbStats>()
               .AsQueryable()
               .FirstOrDefaultAsync(x => x.ChapterId == chapterId &&
                                         x.ProfileId == profileId);
        }

        public async Task<IEnumerable<DbStats>> QueryStats(Guid profileId)
        {
            return await _db
               .Collection<DbStats>()
               .AsQueryable()
               .Where(x => x.ProfileId == profileId)
               .ToListAsync();
        }
    }
}
