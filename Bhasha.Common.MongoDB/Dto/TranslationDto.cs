using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [BsonIgnoreExtraElements]
    public class TranslationDto
    {
        [BsonElement]
        public string Label { get; set; } = "";

        [BsonElement]
        public int SequenceNumber { get; set; }

        [BsonElement]
        public int GroupId { get; set; }

        [BsonElement]
        public string[] Categories { get; set; } = new string[0];

        [BsonElement]
        public string Level { get; set; } = "";

        [BsonElement]
        public string? PictureId { get; set; }

        [BsonElement]
        public string TokenType { get; set; } = "";

        [BsonElement]
        public TokenDto[] Tokens { get; set; } = new TokenDto[0];
    }
}
