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
        public string Name { get; set; } = string.Empty;

        [BsonElement]
        public string Description { get; set; } = string.Empty;

        [BsonElement]
        public PageDto[] Pages { get; set; } = new PageDto[0];

        [BsonElement]
        public string? PictureId { get; set; }
    }
}
