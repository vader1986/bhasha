using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class LanguageTokenDto
    {
        [BsonElement]
        public string Native { get; set; } = string.Empty;

        [BsonElement]
        public string Spoken { get; set; } = string.Empty;

        [BsonElement]
        public string? AudioId { get; set; }
    }
}
