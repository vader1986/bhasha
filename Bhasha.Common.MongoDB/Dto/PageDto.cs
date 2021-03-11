using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class PageDto
    {
        [BsonElement]
        public Guid TokenId { get; set; }

        [BsonElement]
        public string Language { get; set; }

        [BsonElement]
        public string PageType { get; set; }

        [BsonElement]
        public string[] Arguments { get; set; }
    }
}
