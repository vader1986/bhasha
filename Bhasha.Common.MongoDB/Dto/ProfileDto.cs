using System;
using Bhasha.Common.MongoDB.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Profiles)]
    public class ProfileDto : Dto, IEquatable<Profile>
    {
        [BsonElement]
        public string UserId { get; set; } = string.Empty;

        [BsonElement]
        public string From { get; set; } = string.Empty;

        [BsonElement]
        public string To { get; set; } = string.Empty;

        [BsonElement]
        public int Level { get; set; }

        [BsonElement]
        public int CompletedChapters { get; set; }

        public bool Equals(Profile other)
        {
            return other != null && other.Id == Id && other.UserId == UserId && other.From == From && other.To == To && other.Level == Level && other.CompletedChapters == CompletedChapters;
        }
    }
}
