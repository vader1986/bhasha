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

            await db.CreateCollectionAsync(Names.Collections.Profiles);
            await db.CreateCollectionAsync(Names.Collections.Chapters);
            await db.CreateCollectionAsync(Names.Collections.Tokens);
            await db.CreateCollectionAsync(Names.Collections.Translations);
            await db.CreateCollectionAsync(Names.Collections.Stats);

            var profiles = db.GetCollection<ProfileDto>(Names.Collections.Profiles);
            await profiles.CreateIndices(x => x.UserId);

            var chapters = db.GetCollection<GenericChapterDto>(Names.Collections.Chapters);
            await chapters.CreateIndices(x => x.Level);

            var tokens = db.GetCollection<TokenDto>(Names.Collections.Tokens);
            await tokens.CreateIndices(x => x.Label);

            var translations = db.GetCollection<TranslationDto>(Names.Collections.Translations);
            await translations.CreateIndices(x => x.Language, x => x.TokenId);

            var stats = db.GetCollection<ChapterStatsDto>(Names.Collections.Stats);
            await stats.CreateIndices(x => x.ProfileId, x => x.ChapterId);

            return db;
        }
    }
}
