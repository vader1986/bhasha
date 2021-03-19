using System;
using Bhasha.Common.MongoDB.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Users)]
    public class UserDto : Dto, IEquatable<User>
    {
        [BsonElement]
        public string UserName { get; set; } = string.Empty;

        [BsonElement]
        public string Email { get; set; } = string.Empty;

        public bool Equals(User other)
        {
            return other != null && other.Id == Id && other.UserName == UserName && other.Email == Email;
        }
    }
}
