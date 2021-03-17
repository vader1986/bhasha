using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Translations)]
    public class TranslationDto : Dto
    {
        [BsonElement]
        public Guid TokenId { get; set; }

        [BsonElement]
        public string Language { get; set; } = string.Empty;

        [BsonElement]
        public string Native { get; set; } = string.Empty;

        [BsonElement]
        public string Spoken { get; set; } = string.Empty;

        [BsonElement]
        public string? AudioId { get; set; }
    }
}
