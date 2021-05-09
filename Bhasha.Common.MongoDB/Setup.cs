using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.MongoDB.Extensions;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public static class Setup
    {
        public static async Task<IMongoDatabase> NewDatabase(MongoClient client)
        {
            var db = client.GetDatabase(Names.Database);

            await db.CreateCollectionAsync(nameof(DbChapter));
            await db.CreateCollectionAsync(nameof(DbExpression));
            await db.CreateCollectionAsync(nameof(DbStats));
            await db.CreateCollectionAsync(nameof(DbTranslatedChapter));
            await db.CreateCollectionAsync(nameof(DbUserProfile));
            await db.CreateCollectionAsync(nameof(DbWord));

            var profiles = db.GetCollection<DbUserProfile>(nameof(DbUserProfile));
            await profiles.CreateIndices(x => x.UserId!);

            var chapters = db.GetCollection<DbChapter>(nameof(DbChapter));
            await chapters.CreateIndices(x => x.Level);

            var stats = db.GetCollection<DbStats>(nameof(DbStats));
            await stats.CreateIndices(x => x.ProfileId, x => x.ChapterId);

            var translatedChapters = db.GetCollection<DbTranslatedChapter>(nameof(DbTranslatedChapter));
            await translatedChapters.CreateIndices(
                x => x.ChapterId,
                x => x.Languages!.Native!,
                x => x.Languages!.Target!);

            return db;
        }
    }
}
