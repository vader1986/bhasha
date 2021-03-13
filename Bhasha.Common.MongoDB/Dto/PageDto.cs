using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class PageDto
    {
        [BsonElement]
        public Guid TokenId { get; set; }

        [BsonElement]
        public string Language { get; set; } = string.Empty;

        [BsonElement]
        public string PageType { get; set; } = string.Empty;

        [BsonElement]
        public string[] Arguments { get; set; } = new string[0];
    }
}
