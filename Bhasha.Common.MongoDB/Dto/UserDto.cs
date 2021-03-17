using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Users)]
    public class UserDto : Dto
    {
        [BsonElement]
        public string UserName { get; set; } = string.Empty;

        [BsonElement]
        public string Email { get; set; } = string.Empty;
    }
}
