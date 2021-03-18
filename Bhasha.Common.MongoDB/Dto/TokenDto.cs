using System;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    [MongoCollection(Names.Collections.Tokens)]
    public class TokenDto : Dto, IEquatable<Token>
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

        public bool Equals(Token other)
        {
            return other != null && other.Label == Label && other.Level == Level && other.Cefr.ToString() == Cefr && other.TokenType.ToString() == TokenType && other.Categories.Length == Categories.Length && other.Categories.Select((x, i) => x == Categories[i]).All(x => x) && other.PictureId == PictureId;
        }
    }
}
