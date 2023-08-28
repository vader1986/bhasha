using Bhasha.Infrastructure.Mongo.Dtos;
using Bhasha.Infrastructure.Mongo.Extensions;
using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;

namespace Bhasha.Infrastructure.Mongo.Migrations;

public class AddSchemasAndIndices : DatabaseMigration
{
    public AddSchemasAndIndices() : base("1.0.0")
    {
    }

    public override void Up(IMongoDatabase db)
    {
        db.CreateCollection(nameof(ProfileDto));
        db.CreateCollection(nameof(ChapterDto));
        db.CreateCollection(nameof(ExpressionDto));
        db.CreateCollection(nameof(TranslationDto));

        var profiles = db.GetCollection<ProfileDto>("profiles");
        profiles.CreateIndices(x => x.Key.UserId);

        var chapters = db.GetCollection<ChapterDto>("chapters");
        chapters.CreateIndices(x => x.RequiredLevel);

        var expressions = db.GetCollection<ExpressionDto>("expressions");
        expressions.CreateIndices(x => x.Level);

        var translations = db.GetCollection<TranslationDto>("translations");
        translations.CreateIndices(x => x.ExpressionId, x => x.Text);
    }

    public override void Down(IMongoDatabase db)
    {
        db.DropCollection("profiles");
        db.DropCollection("chapters");
        db.DropCollection("expressions");
        db.DropCollection("translations");
    }
}

