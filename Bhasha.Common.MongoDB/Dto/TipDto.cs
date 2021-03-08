using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class TipDto
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement]
        public Guid ChapterId { get; set; }

        [BsonElement]
        public int PageIndex { get; set; }

        [BsonElement]
        public string Text { get; set; }
    }
}
