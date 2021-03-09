using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class ChapterStatsDto
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement]
        public Guid ProfileId { get; set; }

        [BsonElement]
        public Guid ChapterId { get; set; }

        [BsonElement]
        public bool Completed { get; set; }

        [BsonElement]
        public string Tips { get; set; }

        [BsonElement]
        public string Submits { get; set; }

        [BsonElement]
        public string Failures { get; set; }
    }
}
