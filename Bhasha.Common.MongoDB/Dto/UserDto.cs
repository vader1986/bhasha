using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class UserDto
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement]
        public string UserName { get; set; }

        [BsonElement]
        public string Email { get; set; }
    }
}
