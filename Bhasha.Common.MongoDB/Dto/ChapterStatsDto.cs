using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class ChapterStatsDto
    {
        [BsonId, BsonIgnoreIfDefault]
        public Guid Id { get; set; }

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
    }
}
