using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Tokens)]
    public class TokenDto : Dto
    {
        [BsonElement]
        public string Label { get; set; } = string.Empty;

        [BsonElement]
        public int Level { get; set; }

        [BsonElement]
        public string Cefr { get; set; } = string.Empty;

        [BsonElement]
        public string TokenType { get; set; } = string.Empty;

        [BsonElement]
        public string[] Categories { get; set; } = new string[0];

        [BsonElement]
        public string? PictureId { get; set; }
    }
}
