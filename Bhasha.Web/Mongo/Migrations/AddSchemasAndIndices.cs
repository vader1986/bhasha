using Bhasha.Web.Domain;
using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;

namespace Bhasha.Web.Mongo.Migrations;

public class AddSchemasAndIndices : DatabaseMigration
{
    public AddSchemasAndIndices() : base("1.0.0")
    {
    }

    public override void Up(IMongoDatabase db)
    {
        db.CreateCollection(nameof(Profile));
        db.CreateCollection(nameof(Chapter));
        db.CreateCollection(nameof(Expression));
        db.CreateCollection(nameof(Translation));

        var profiles = db.GetCollection<Profile>(nameof(Profile));
        profiles.CreateIndices(x => x.UserId);

        var chapters = db.GetCollection<Chapter>(nameof(Chapter));
        chapters.CreateIndices(x => x.RequiredLevel);

        var expressions = db.GetCollection<Expression>(nameof(Expression));
        expressions.CreateIndices(x => x.Level);

        var translations = db.GetCollection<Translation>(nameof(Translation));
        translations.CreateIndices(x => x.ExpressionId, x => x.Language, x => x.Native);
    }

    public override void Down(IMongoDatabase db)
    {
        db.DropCollection(nameof(Profile));
        db.DropCollection(nameof(Chapter));
        db.DropCollection(nameof(Expression));
        db.DropCollection(nameof(Translation));
    }
}

