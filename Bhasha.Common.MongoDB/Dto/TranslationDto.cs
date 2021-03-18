using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Translations)]
    public class TranslationDto : Dto, IEquatable<Translation>
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

        public bool Equals(Translation other)
        {
            return other != null && other.Id == Id && other.TokenId == TokenId && other.Language == Language && other.Native == Native && other.Spoken == Spoken && other.AudioId == AudioId;
        }
    }
}
