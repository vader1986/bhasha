using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class GenericPageDto
    {
        [BsonElement]
        public Guid TokenId { get; set; }

        [BsonElement]
        public string PageType { get; set; } = string.Empty;
    }
}
