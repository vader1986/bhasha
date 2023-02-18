using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Bhasha.Infrastructure.Mongo;

public static class MongoSetup
{
    public static void Configure()
    {
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }
}

