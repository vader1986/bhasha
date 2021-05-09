using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bhasha.Common.Database
{
    public interface IDatabase
    {
        Task<DbStats?> QueryStats(Guid chapterId, Guid profileId);
        Task<IEnumerable<DbStats>> QueryStats(Guid profileId);
        Task<IEnumerable<DbChapter>> QueryChapters(int level);
        Task<IEnumerable<DbUserProfile>> QueryProfiles(string userId);
    }
}
