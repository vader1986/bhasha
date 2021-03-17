using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Tips)]
    public class TipDto : Dto
    {
        [BsonElement]
        public Guid ChapterId { get; set; }

        [BsonElement]
        public int PageIndex { get; set; }

        [BsonElement]
        public string Text { get; set; } = string.Empty;
    }
}
