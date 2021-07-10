using IdentityServer4.Models;
using Mongo.Migration.Migrations.Database;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Bhasha.Identity.Mongo.Migrations
{
    public class AddInitialResources : DatabaseMigration
    {
        public AddInitialResources() : base("1.0.0")
        {
        }

        public override void Up(IMongoDatabase db)
        {
            var clientTable = db.GetCollection<Client>(typeof(Client).Name);
            foreach (var client in Config.Clients)
            {
                clientTable.InsertOne(client);
            }

            var identityResourceTable = db.GetCollection<IdentityResource>(typeof(IdentityResource).Name);
            foreach (var res in Config.IdentityResources)
            {
                identityResourceTable.InsertOne(res);
            }

            var scopeTable = db.GetCollection<ApiScope>(typeof(ApiScope).Name);
            foreach (var scope in Config.ApiScopes)
            {
                scopeTable.InsertOne(scope);
            }
        }

        public override void Down(IMongoDatabase db)
        {
            db.DropCollection(typeof(Client).Name);
            db.DropCollection(typeof(IdentityResource).Name);
            db.DropCollection(typeof(ApiScope).Name);
        }
    }
}
