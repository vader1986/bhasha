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

            await db.CreateCollectionAsync(Names.Collections.Translations);

            var translations = db.GetCollection<TranslationDto>(Names.Collections.Translations);

            await translations.CreateIndices(
                x => x.Categories,
                x => x.Level,
                x => x.Label,
                x => x.TokenType);

            await translations.CreateIndices("Tokens.LanguageId");
            
            await db.CreateCollectionAsync(Names.Collections.Procedures);

            var procedures = db.GetCollection<ProcedureDto>(Names.Collections.Procedures);

            await procedures.CreateIndices(
                x => x.ProcedureId,
                x => x.Support);

            return db;
        }
    }
}
