using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Bhasha.Common.MongoDB.Dto
{
    using Translations = Dictionary<string, LanguageTokenDto>;

    public class TokenDto
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement]
        public string Label { get; set; }

        [BsonElement]
        public int Level { get; set; }

        [BsonElement]
        public string Cefr { get; set; }

        [BsonElement]
        public string TokenType { get; set; }

        [BsonElement]
        public string[] Categories { get; set; }

        [BsonElement]
        public string? PictureId { get; set; }

        [BsonElement]
        public Translations Translations { get; set; }
    }
}
