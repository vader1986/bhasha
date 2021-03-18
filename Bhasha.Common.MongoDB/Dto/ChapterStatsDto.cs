using System;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Stats)]
    public class ChapterStatsDto : Dto, IEquatable<ChapterStats>
    {
        [BsonElement]
        public Guid ProfileId { get; set; }

        [BsonElement]
        public Guid ChapterId { get; set; }

        [BsonElement]
        public bool Completed { get; set; }

        [BsonElement]
        public string Tips { get; set; } = string.Empty;

        [BsonElement]
        public string Submits { get; set; } = string.Empty;

        [BsonElement]
        public string Failures { get; set; } = string.Empty;

        public bool Equals(ChapterStats other)
        {
            return other != null && other.Id == Id && other.ProfileId == ProfileId && other.ChapterId == ChapterId && other.Completed == Completed && Tips == Encoding.UTF8.GetString(other.Tips) && Submits == Encoding.UTF8.GetString(other.Submits) && Failures == Encoding.UTF8.GetString(other.Failures);
        }
    }
}
