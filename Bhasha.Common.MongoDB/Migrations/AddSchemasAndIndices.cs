using Bhasha.Common.Database;
using Bhasha.Common.MongoDB.Extensions;
using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Migrations
{
    public class AddSchemasAndIndices : DatabaseMigration
    {
        public AddSchemasAndIndices() : base("1.0.0")
        {
        }

        public override void Up(IMongoDatabase db)
        {
            db.CreateCollection(nameof(DbChapter));
            db.CreateCollection(nameof(DbExpression));
            db.CreateCollection(nameof(DbStats));
            db.CreateCollection(nameof(DbTranslatedChapter));
            db.CreateCollection(nameof(DbUserProfile));
            db.CreateCollection(nameof(DbWord));

            var profiles = db.GetCollection<DbUserProfile>(nameof(DbUserProfile));
            profiles.CreateIndices(x => x.UserId!);

            var chapters = db.GetCollection<DbChapter>(nameof(DbChapter));
            chapters.CreateIndices(x => x.Level);

            var stats = db.GetCollection<DbStats>(nameof(DbStats));
            stats.CreateIndices(x => x.ProfileId, x => x.ChapterId);

            var translatedChapters = db.GetCollection<DbTranslatedChapter>(nameof(DbTranslatedChapter));
            translatedChapters.CreateIndices(
                x => x.Languages!.Native!,
                x => x.Languages!.Target!);
        }

        public override void Down(IMongoDatabase db)
        {
            db.DropCollection(nameof(DbChapter));
            db.DropCollection(nameof(DbExpression));
            db.DropCollection(nameof(DbStats));
            db.DropCollection(nameof(DbTranslatedChapter));
            db.DropCollection(nameof(DbUserProfile));
            db.DropCollection(nameof(DbWord));
        }
    }
}
