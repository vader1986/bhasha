using Bhasha.Web.Domain;
using Mongo.Migration.Migrations.Database;
using MongoDB.Driver;

namespace Bhasha.Web.Mongo.Migrations
{
    public class AddSchemasAndIndices : DatabaseMigration
    {
        public AddSchemasAndIndices() : base("1.0.0")
        {
        }

        public override void Up(IMongoDatabase db)
        {
            db.CreateCollection(nameof(Profile));
            db.CreateCollection(nameof(Expression));
            db.CreateCollection(nameof(Chapter));

            var profiles = db.GetCollection<Profile>(nameof(Profile));
            profiles.CreateIndices(x => x.UserId);

            var expressions = db.GetCollection<Chapter>(nameof(Chapter));
            expressions.CreateIndices(x => x.RequiredLevel);
        }

        public override void Down(IMongoDatabase db)
        {
            db.DropCollection(nameof(Chapter));
            db.DropCollection(nameof(Expression));
            db.DropCollection(nameof(Profile));
        }
    }
}

