using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class ChapterDto
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement]
        public int Level { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public string Description { get; set; }

        [BsonElement]
        public PageDto[] Pages { get; set; }

        [BsonElement]
        public string? PictureId { get; set; }
    }
}
