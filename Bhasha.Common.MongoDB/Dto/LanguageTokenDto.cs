using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    public class LanguageTokenDto
    {
        [BsonElement]
        public string Native { get; set; }

        [BsonElement]
        public string Spoken { get; set; }

        [BsonElement]
        public string? AudioId { get; set; }
    }
}
