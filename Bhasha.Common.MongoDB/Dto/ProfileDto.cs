using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Profiles)]
    public class ProfileDto : Dto
    {
        [BsonElement]
        public Guid UserId { get; set; }

        [BsonElement]
        public string From { get; set; } = string.Empty;

        [BsonElement]
        public string To { get; set; } = string.Empty;

        [BsonElement]
        public int Level { get; set; }
    }
}
