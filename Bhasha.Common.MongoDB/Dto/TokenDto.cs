using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [BsonIgnoreExtraElements]
    public class TokenDto
    {
        [BsonElement]
        public string LanguageId { get; set; } = "";

        [BsonElement]
        public string Native { get; set; } = "";

        [BsonElement]
        public string Spoken { get; set; } = "";

        [BsonElement]
        public string? AudioId { get; set; }
    }
}
