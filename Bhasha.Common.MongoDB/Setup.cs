using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.MongoDB.Extensions;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public static class Setup
    {
        public static async Task<IMongoDatabase> NewDatabase(MongoClient client)
        {
            var db = client.GetDatabase(Names.Database);

            await db.CreateCollectionAsync(Names.Collections.Users);
            await db.CreateCollectionAsync(Names.Collections.Profiles);
            await db.CreateCollectionAsync(Names.Collections.Chapters);
            await db.CreateCollectionAsync(Names.Collections.Tokens);
            await db.CreateCollectionAsync(Names.Collections.Tips);
            await db.CreateCollectionAsync(Names.Collections.Stats);

            var profiles = db.GetCollection<ProfileDto>(Names.Collections.Profiles);
            await profiles.CreateIndices(x => x.UserId);

            var chapters = db.GetCollection<ChapterDto>(Names.Collections.Chapters);
            await chapters.CreateIndices(x => x.Level);

            var tips = db.GetCollection<TipDto>(Names.Collections.Tips);
            await tips.CreateIndices(x => x.ChapterId, x => x.PageIndex);

            var stats = db.GetCollection<ChapterStatsDto>(Names.Collections.Stats);
            await stats.CreateIndices(x => x.ProfileId, x => x.ChapterId);

            return db;
        }
    }
}
