using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public abstract class Dto
    {
        [BsonId, BsonIgnoreIfDefault]
        public Guid Id { get; set; }
    }
}
